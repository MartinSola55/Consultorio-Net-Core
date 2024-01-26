using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Consultorio.Models.ViewModels.Turnos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Consultorio.Controllers
{
    public class TurnosController(IWorkContainer workContainer) : Controller
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
                    Turnos = _workContainer.Turno.GetAll(includeProperties: "Persona, DiaHorario, Persona.ObraSocial, DiaHorario.Horario"),
                    Horarios = _workContainer.DiaHorario.GetHorariosByDate(DateTime.UtcNow.AddHours(-3)),
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
        [ActionName("SearchByDate")]
        public IActionResult SearchByDate(string dateString)
        {
            try
            {
                DateTime date = DateTime.Parse(dateString);
                List<Turno> turnos = _workContainer.Turno.GetByDate(date);
                List<DiaHorario> horarios = _workContainer.DiaHorario.GetHorariosByDate(date);
                List<object> data = [];
                foreach (DiaHorario diaHorario in horarios)
                {
                    if (turnos.Any(x => x.DiaHorario.HorarioID == diaHorario.HorarioID))
                    {
                        Turno turno = turnos.First(x => x.DiaHorario.HorarioID == diaHorario.HorarioID);
                        data.Add(new
                        {
                            nombre = turno.Persona.Nombre,
                            apellido = turno.Persona.Apellido,
                            obraSocial = turno.Persona.ObraSocial.Nombre,
                            telefono = turno.Persona.Telefono,
                            hora = turno.DiaHorario.Horario.Hora.ToString("HH:mm"),
                            disponible = false,
                            diaHorarioID = diaHorario.ID,
                        });
                    } else
                    {
                        data.Add(new
                        {
                            nombre = "-",
                            apellido = "-",
                            obraSocial = "-",
                            telefono = "-",
                            hora = diaHorario.Horario.Hora.ToString("HH:mm"),
                            disponible = diaHorario.Disponible,
                            diaHorarioID = diaHorario.ID,
                        });
                    }
                }

                return Json(new
                {
                    success = true,
                    data
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se encontraron los turnos", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public IActionResult Create(Turno turno)
        {
            try
            {
                ModelState.Remove("turno.DiaHorario");
                ModelState.Remove("turno.ObraSocial");
                ModelState.Remove("turno.Persona.ObraSocial");
                if (ModelState.IsValid)
                {
                    var newTurno = _workContainer.Turno.CreateTurno(turno);

                    object data = new
                    {
                        nombre = newTurno.Persona.Nombre,
                        apellido = newTurno.Persona.Apellido,
                        obraSocial = newTurno.Persona.ObraSocial.Nombre,
                        telefono = newTurno.Persona.Telefono,
                        hora = newTurno.DiaHorario.Horario.Hora.ToString("HH:mm"),
                        disponible = false,
                        diaHorarioID = newTurno.DiaHorarioID,
                    };
                    
                    return Json(new
                    {
                        success = true,
                        data,
                        message = "El turno se ha agregado correctamente",
                    });
                }
                return CustomBadRequest(title: "No se pudo agregar el turno", message: "Alguno de los datos ingresados no es válido");
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se pudo agregar el turno", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteTurno")]
        public IActionResult DeleteTurno(long diaHorarioID)
        {
            try
            {
                var turno = _workContainer.Turno.GetFirstOrDefault(x => x.ID == diaHorarioID);
                _workContainer.Turno.SoftDelete(diaHorarioID);
                var horario = _workContainer.DiaHorario.GetFirstOrDefault(x => x.ID == diaHorarioID, includeProperties: "Horario");
                object data = new
                {
                    id = horario.ID,
                    hora = horario.Horario.Hora.ToString("HH:mm"),
                };
                return Json(new
                {
                    success = true,
                    data,
                    message = "El turno se ha eliminado correctamente",
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se pudo eliminar el turno", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteHorario")]
        public IActionResult DeleteHorario(long diaHorarioID)
        {
            try
            {
                var horario = _workContainer.DiaHorario.GetFirstOrDefault(x => x.ID == diaHorarioID, includeProperties: "Horario");
                object data = new
                {
                    id = horario.ID,
                    hora = horario.Horario.Hora.ToString("HH:mm"),
                };
                _workContainer.DiaHorario.SoftDelete(diaHorarioID);
                return Json(new
                {
                    success = true,
                    data,
                    message = "El horario se ha eliminado correctamente",
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se pudo eliminar el horario", message: "Intente nuevamente o comuníquese para soporte");
            }
        }
    }
}
