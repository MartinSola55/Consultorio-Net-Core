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

        [HttpGet]
        [ActionName("Index")]
        public IActionResult Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Pacientes = _workContainer.Paciente.GetAll(includeProperties: "ObraSocial"),
                    ObrasSociales = _workContainer.ObraSocial.GetDropDownList(),
                };
                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpGet]
        [ActionName("Detalles")]
        public IActionResult Detalles(long id)
        {
            try
            {
                var paciente = _workContainer.Paciente.GetFirstOrDefault(p => p.ID == id, includeProperties: "ObraSocial, HistoriasClinicas");
                if (paciente is null) return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Error al obtener el paciente\nEl paciente no existe", ErrorCode = 404 });

                DetallesViewModel viewModel = new()
                {
                    Paciente = paciente,
                    ObrasSociales = _workContainer.ObraSocial.GetDropDownList(),
                };
                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public IActionResult Create(Paciente paciente)
        {
            try
            {
                ModelState.Remove("paciente.HistoriasClinicas");
                ModelState.Remove("paciente.ObraSocial");
                if (!ModelState.IsValid)
                    return CustomBadRequest(title: "Error al guardar el paciente", message: "Alguno de los datos ingresados no es correcto");

                if (_workContainer.Paciente.IsDuplicated(paciente))
                    return CustomBadRequest(title: "Error al guardar el paciente", message: "Ya existe un paciente con el mismo nombre, apellido y fecha de nacimiento");
                
                _workContainer.Paciente.Add(paciente);
                _workContainer.Save();

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
        [ActionName("SearchByDate")]
        public IActionResult SearchByDate(string date)
        {
            try
            {
                DateTime nacimiento = DateTime.Parse(date);
                List<Paciente> pacientes = _workContainer.Paciente.GetByNacimiento(nacimiento);
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
                        ObraSocial = x.ObraSocialID != null ? x.ObraSocial.Nombre : "-",
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
        [ActionName("SearchByName")]
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
        [ActionName("UpdateDatos")]
        public IActionResult UpdateDatos(string datoToUpdate, string datoValue, long pacienteID)
        {
            try
            {
                _workContainer.Paciente.UpdateDatos(datoToUpdate, datoValue, pacienteID);
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
        [ActionName("UpdateHC")]
        public IActionResult UpdateHC(HistoriaClinica historiaClinica)
        {
            try
            {
                _workContainer.Paciente.UpdateHC(historiaClinica);
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
        [ActionName("SaveHC")]
        public IActionResult SaveHC(HistoriaClinica historiaClinica)
        {
            try
            {
                long id = _workContainer.Paciente.AddHC(historiaClinica);
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
        [ActionName("SoftDelete")]
        public IActionResult SoftDelete(long id)
        {
            try
            {
                _workContainer.Paciente.SoftDelete(id);
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
    }
}
