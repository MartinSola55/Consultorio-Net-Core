using Consultorio.Data.Repository.IRepository;
using Consultorio.Data.Repository;
using Consultorio.Models;
using Microsoft.AspNetCore.Mvc;
using Consultorio.Models.ViewModels.Horarios;
using NuGet.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Policy;

namespace Consultorio.Controllers
{
    public class HorariosController(IWorkContainer workContainer) : Controller
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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Horarios = await _workContainer.Horario.GetAllAsync()
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
        [Authorize]
        public async Task<IActionResult> Save(string horarios, string dateFrom, string dateTo)
        {
            try
            {
                short[] ids = [.. horarios.Split(',').Select(x => short.Parse(x))];
                await _workContainer.DiaHorario.SaveNew(ids, dateFrom, dateTo);
                return Json(new
                {
                    success = true,
                    message = "Se han guardado los horarios correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error", message: "No se ha podido guardar los horarios", error: e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDisponibles(string dateString)
        {
            try
            {
                DateTime date = DateTime.Parse(dateString);

                if (User.Identity is not null && User.Identity.IsAuthenticated)
                {
                    DateTime today = DateTime.UtcNow.AddHours(-3);
                    if (date.Date < today.Date || date.Date > today.AddDays(Constants.MaximosDiasReserva).Date) throw new PolicyException("Debes ingresar una fecha válida");
                }
                var horarios = await _workContainer.Horario.GetDisponibles(date);
                return Json(new
                {
                    success = true,
                    data = horarios.Select(x => new
                    {
                        id = x.ID,
                        hora = x.Horario.Hora.ToString("HH:mm"),
                    })
                });
            }
            catch (PolicyException e)
            {
                return CustomBadRequest(title: "No se pueden buscar los horarios", message: e.Message);
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al buscar los horarios", message: "No se han podido obtener los horarios disponibles", error: e.Message);
            }
        }
        
        #endregion
    }
}
