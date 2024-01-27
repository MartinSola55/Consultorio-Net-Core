using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Consultorio.Models.ViewModels.ObrasSociales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Consultorio.Controllers
{
    [Authorize]
    public class ObrasSocialesController(IWorkContainer workContainer) : Controller
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
                    ObraSociales = _workContainer.ObraSocial.GetAll().OrderByDescending(x => x.Nombre),
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("~/Views/Error.cshtml", new ErrorViewModel { Message = "Ha ocurrido un error inesperado con el servidor\nSi sigue obteniendo este error contacte a soporte", ErrorCode = 500 });
            }
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ObraSocial obraSocial = viewModel.NewObraSocial;
                    if (_workContainer.ObraSocial.IsDuplicated(obraSocial))
                    {
                        return BadRequest(new
                        {
                            success = false,
                            title = "Error al agregar la obra social",
                            message = "Ya existe otra con el mismo nombre",
                        });
                    }
                    _workContainer.ObraSocial.Add(obraSocial);
                    _workContainer.Save();

                    object data = new
                    {
                        id = obraSocial.ID,
                        nombre = obraSocial.Nombre,
                        habilitada = obraSocial.Habilitada,
                    };
                    return Json(new
                    {
                        success = true,
                        data,
                        message = "La obra social se creó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al crear la obra social", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al crear la obra social", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ObraSocial obraSocial = viewModel.NewObraSocial;

                    if (_workContainer.ObraSocial.IsDuplicated(obraSocial))
                    {
                        return BadRequest(new
                        {
                            success = false,
                            title = "Error al editar la obra social",
                            message = "Ya existe otra con el mismo nombre",
                        });
                    }

                    _workContainer.ObraSocial.Update(obraSocial);
                    return Json(new
                    {
                        success = true,
                        data = obraSocial,
                        message = "La obra social se editó correctamente",
                    });
                }
                catch (Exception e)
                {
                    return CustomBadRequest(title: "Error al editar la obra social", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
                }
            }
            return CustomBadRequest(title: "Error al editar la obra social", message: "Alguno de los campos ingresados no es válido");
        }

        [HttpPost]
        [ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        public IActionResult SoftDelete(long id)
        {
            try
            {
                _workContainer.ObraSocial.SoftDelete(id);

                return Json(new
                {
                    success = true,
                    data = id,
                    message = "La obra social se eliminó correctamente",
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al eliminar la obra social", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

        [HttpPost]
        [ActionName("ChangeState")]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeState(long id)
        {
            try
            {
                _workContainer.ObraSocial.ChangeState(id);

                ObraSocial obraSocial = _workContainer.ObraSocial.GetFirstOrDefault(x => x.ID == id);

                return Json(new
                {
                    success = true,
                    data = obraSocial,
                    message = "La obra social se " + (obraSocial.Habilitada ? "habilitó" : "deshabilitó"),
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "Error al deshabilitar la obra social", message: "Intente nuevamente o comuníquese para soporte", error: e.Message);
            }
        }

    }
}
