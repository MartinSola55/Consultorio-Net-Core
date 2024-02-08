using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Consultorio.Models.ViewModels.Alertas;
using Microsoft.AspNetCore.Mvc;

namespace Consultorio.Controllers
{
    public class AlertasController(IWorkContainer workContainer) : Controller
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

        public async Task<IActionResult> Index()
        {
            try
            {
                IndexViewModel viewModel = new()
                {
                    Alertas = await _workContainer.Alerta.GetAllAsync()
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
        public async Task<IActionResult> Create([Bind("Mensaje, Desde, Hasta")] Alerta alerta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _workContainer.Alerta.AddAsync(alerta);
                    await _workContainer.SaveAsync();
                    return Json(new 
                    {
                        success = true,
                        data = new
                        {
                            id = alerta.ID,
                            mensaje = alerta.Mensaje,
                            desde = alerta.Desde.ToString("dd/MM/yyyy"),
                            hasta = alerta.Hasta.ToString("dd/MM/yyyy"),
                        },
                        message = "Alerta creada correctamente"
                    });
                }
                return CustomBadRequest(title: "Error al crear la alerta", message: "Alguno de los campos ingresados no es válido");
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al crear la alerta", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(long id)
        {
            try
            {
                await _workContainer.Alerta.SoftDelete(id);

                return Json(new
                {
                    success = true,
                    data = id,
                    message = "La alerta se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar la alerta", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        #endregion
    }
}
