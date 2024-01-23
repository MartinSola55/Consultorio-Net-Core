using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Consultorio.Models.ViewModels.Pacientes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Consultorio.Controllers
{
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
                    Pacientes = _workContainer.Paciente.GetAll(includeProperties: "ObraSocial")
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
                DetallesViewModel viewModel = new()
                {
                    Paciente = _workContainer.Paciente.GetFirstOrDefault(p => p.ID == id, includeProperties: "ObraSocial, HistoriasClinicas"),
                    ObraSociales = _workContainer.ObraSocial.GetDropDownList(),
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
    }
}
