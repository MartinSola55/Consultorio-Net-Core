using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Consultorio.Models.ViewModels.Pacientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Consultorio.Controllers
{
    [Authorize]
    public class PacientesController(IWorkContainer workContainer) : Controller
    {
        private readonly IWorkContainer _workContainer = workContainer;

        private BadRequestObjectResult CustomBadRequest(string title, string message, string? error = null)
        {
            return BadRequest(new
            {
                success = false,
                title,
                message,
                error,
            });
        }

        #region Views

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    ObrasSociales = await _workContainer.ObraSocial.GetDropDownList(),
                };
                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(long id)
        {
            try
            {
                var paciente = await _workContainer.Paciente.GetFirstOrDefaultAsync(p => p.ID == id, includeProperties: "ObraSocial, HistoriasClinicas");
                if (paciente is null) return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener el paciente\nEl paciente no existe", ErrorCode = 404 });

                DetallesViewModel viewModel = new()
                {
                    Paciente = paciente,
                    ObrasSociales = await _workContainer.ObraSocial.GetDropDownList(),
                };
                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        #endregion

        #region Actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            try
            {
                ModelState.Remove("paciente.HistoriasClinicas");
                ModelState.Remove("paciente.ObraSocial");
                if (!ModelState.IsValid)
                    return CustomBadRequest(title: "Error al guardar el paciente", message: "Alguno de los datos ingresados no es correcto");

                if (await _workContainer.Paciente.IsDuplicated(paciente))
                    return CustomBadRequest(title: "Error al guardar el paciente", message: "Ya existe un paciente con el mismo nombre, apellido y fecha de nacimiento");

                await _workContainer.Paciente.AddAsync(paciente);
                await _workContainer.SaveAsync();

                return Json(new
                {
                    success = true,
                    data = paciente.ID,
                    message = "El paciente se guardó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al guardar el paciente", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchByDate(string date)
        {
            try
            {
                DateTime nacimiento = DateTime.Parse(date);
                List<Paciente> pacientes = await _workContainer.Paciente.GetByNacimiento(nacimiento);
                return Json(new
                {
                    success = true,
                    data = pacientes.Select(x => new
                    {
                        Id = x.ID,
                        Nombre = x.Nombre,
                        Apellido = x.Apellido,
                        Telefono = x.Telefono ?? "-",
                        Direccion = x.Direccion ?? "-",
                        Localidad = x.Localidad ?? "-",
                        FechaNacimiento = x.FechaNacimiento.ToString("dd/MM/yyyy"),
                        ObraSocial = x.ObraSocial.Nombre,
                        UpdatedAt = x.UpdatedAt.ToString("dd/MM/yyyy")
                    })
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al buscar los pacientes", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchByName(string words)
        {
            try
            {
                Task<List<GetByNameResponse>> pacientes = _workContainer.Paciente.GetByNombreApellido(words);
                return Json(new
                {
                    success = true,
                    data = await pacientes
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al buscar los pacientes", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDatos(string datoToUpdate, string datoValue, long pacienteID)
        {
            try
            {
                if (datoValue is null || datoValue == "")
                    return CustomBadRequest(title: "Error al actualizar el campo", message: "El campo no puede estar vacío");

                await _workContainer.Paciente.UpdateDatos(datoToUpdate, datoValue, pacienteID);
                return Json(new
                {
                    success = true,
                    message = "El campo se actualizó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al actualizar el campo", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(long id)
        {
            try
            {
                await _workContainer.Paciente.SoftDelete(id);
                return Json(new
                {
                    success = true,
                    data = id,
                    message = "El paciente se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar el paciente", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        #endregion

        #region Historias Clinicas

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHC(HistoriaClinica historiaClinica)
        {
            try
            {
                await _workContainer.Paciente.UpdateHC(historiaClinica);
                return Json(new
                {
                    success = true,
                    message = "La historia clínica se actualizó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al actualizar la historia clínica", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveHC(HistoriaClinica historiaClinica)
        {
            try
            {
                long id = await _workContainer.Paciente.AddHC(historiaClinica);
                return Json(new
                {
                    success = true,
                    data = id,
                    message = "La historia clínica se guardó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al crear la obra social", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHC(long id)
        {
            try
            {
                await _workContainer.Paciente.DeleteHC(id);
                return Json(new
                {
                    success = true,
                    data = id,
                    message = "La historia clínica se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar la historia clínica", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        #endregion

    }
}
