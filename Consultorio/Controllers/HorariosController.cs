using Consultorio.Data.Repository.IRepository;
using Consultorio.Data.Repository;
using Consultorio.Models;
using Microsoft.AspNetCore.Mvc;
using Consultorio.Models.ViewModels.Horarios;
using NuGet.Common;

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

        [HttpGet]
        [ActionName("Index")]
        public IActionResult Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Horarios = _workContainer.Horario.GetAll()
                };
                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }

        }

        [HttpPost]
        [ActionName("Save")]
        public IActionResult Save(string horarios, string dateFrom, string dateTo)
        {
            try
            {
                short[] ids = [.. horarios.Split(',').Select(x => short.Parse(x))];
                _workContainer.DiaHorario.SaveNew(ids, dateFrom, dateTo);
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
    }
}
