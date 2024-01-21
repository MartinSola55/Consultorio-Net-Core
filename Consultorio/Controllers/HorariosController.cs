using Consultorio.Data.Repository.IRepository;
using Consultorio.Data.Repository;
using Consultorio.Models;
using Microsoft.AspNetCore.Mvc;
using Consultorio.Models.ViewModels.Horarios;

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
    }
}
