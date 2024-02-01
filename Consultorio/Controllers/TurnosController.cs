using System.Security.Policy;
using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Consultorio.Models.ViewModels.Turnos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

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

        #region Admin

        [HttpGet]
        [ActionName("Index")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                DateTime today = DateTime.UtcNow.AddHours(-3);
                IndexViewModel viewModel = new()
                {
                    Turnos = await _workContainer.Turno.GetAllAsync(x => x.DiaHorario.Dia.Date == today.Date, includeProperties: "Persona, DiaHorario, Persona.ObraSocial, DiaHorario.Horario"),
                    Horarios = await _workContainer.DiaHorario.GetHorariosByDate(DateTime.UtcNow.AddHours(-3)),
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
        [ActionName("SearchByDate")]
        [Authorize]
        public async Task<IActionResult> SearchByDate(string dateString)
        {
            try
            {
                DateTime date = DateTime.Parse(dateString);
                List<Turno> turnos = await _workContainer.Turno.GetByDate(date);
                List<DiaHorario> horarios = await _workContainer.DiaHorario.GetHorariosByDate(date);
                List<object> data = [];
                foreach (DiaHorario diaHorario in horarios)
                {
                    if (turnos.Any(x => x.DiaHorario.HorarioID == diaHorario.HorarioID))
                    {
                        Turno turno = turnos.First(x => x.DiaHorario.HorarioID == diaHorario.HorarioID);
                        data.Add(new
                        {
                            id = turno.ID,
                            nombre = turno.Persona.Nombre,
                            apellido = turno.Persona.Apellido,
                            obraSocial = turno.Persona.ObraSocial.Nombre,
                            telefono = turno.Persona.Telefono,
                            hora = turno.DiaHorario.Horario.Hora.ToString("HH:mm"),
                            disponible = false,
                            diaHorarioID = diaHorario.ID,
                            obraSocialID = turno.Persona.ObraSocialID,
                        });
                    }
                    else
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
        [Authorize]
        public async Task<IActionResult> Create(Turno turno)
        {
            try
            {
                ModelState.Remove("turno.DiaHorario");
                ModelState.Remove("turno.ObraSocial");
                ModelState.Remove("turno.Persona.ObraSocial");
                if (ModelState.IsValid)
                {
                    var newTurno = await _workContainer.Turno.CreateTurno(turno, byPaciente: false);

                    var data = new
                    {
                        id = newTurno.ID,
                        nombre = newTurno.Persona.Nombre,
                        apellido = newTurno.Persona.Apellido,
                        obraSocial = newTurno.Persona.ObraSocial.Nombre,
                        obraSocialID = newTurno.Persona.ObraSocialID,
                        telefono = newTurno.Persona.Telefono,
                        hora = newTurno.DiaHorario.Horario.Hora.ToString("HH:mm"),
                        disponible = false,
                        diaHorarioID = newTurno.DiaHorarioID,
                    };

                    return Json(new
                    {
                        success = true,
                        data,
                        message = "El turno se ha guardado correctamente",
                    });
                }
                return CustomBadRequest(title: "No se pudo guardar el turno", message: "Alguno de los datos ingresados no es válido");
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se pudo guardar el turno", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        [Authorize]
        public async Task<IActionResult> Update(Turno turno)
        {
            try
            {
                ModelState.Remove("turno.DiaHorario");
                ModelState.Remove("turno.ObraSocial");
                ModelState.Remove("turno.Persona.ObraSocial");
                if (ModelState.IsValid)
                {
                    var oldTurno = await _workContainer
                        .Turno
                        .GetFirstOrDefaultAsync(x => x.ID == turno.ID, includeProperties: "DiaHorario.Horario");

                    var oldDiaHorarioID = oldTurno.DiaHorarioID;
                    var oldHora = oldTurno.DiaHorario.Horario.Hora.ToString("HH:mm");
                    var newTurno = await _workContainer.Turno.Update(turno);

                    var data = new
                    {
                        id = newTurno.ID,
                        nombre = newTurno.Persona.Nombre,
                        apellido = newTurno.Persona.Apellido,
                        obraSocial = newTurno.Persona.ObraSocial.Nombre,
                        obraSocialID = newTurno.Persona.ObraSocialID,
                        telefono = newTurno.Persona.Telefono,
                        hora = newTurno.DiaHorario.Horario.Hora.ToString("HH:mm"),
                        disponible = false,
                        diaHorarioID = newTurno.DiaHorarioID,
                        oldDiaHorarioID,
                        oldHora,
                    };
                    return Json(new
                    {
                        success = true,
                        data,
                        message = "El turno se ha actualizado correctamente",
                    });
                }
                return CustomBadRequest(title: "No se pudo actualizar el turno", message: "Alguno de los datos ingresados no es válido");
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se pudo actualizar el turno", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteTurno")]
        [Authorize]
        public async Task<IActionResult> DeleteTurno(long diaHorarioID)
        {
            try
            {
                var turno = await _workContainer
                    .Turno
                    .GetFirstOrDefaultAsync(x => x.ID == diaHorarioID);

                await _workContainer.Turno.SoftDelete(diaHorarioID);

                var horario = await _workContainer
                    .DiaHorario
                    .GetFirstOrDefaultAsync(x => x.ID == diaHorarioID, includeProperties: "Horario");

                var data = new
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
        [Authorize]
        public async Task<IActionResult> DeleteHorario(long diaHorarioID)
        {
            try
            {
                var horario = await _workContainer.DiaHorario.GetFirstOrDefaultAsync(x => x.ID == diaHorarioID, includeProperties: "Horario");
                var data = new
                {
                    id = horario.ID,
                    hora = horario.Horario.Hora.ToString("HH:mm"),
                };
                await _workContainer.DiaHorario.SoftDelete(diaHorarioID);
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

        [HttpGet]
        [ActionName("GetHorariosDisponibles")]
        [Authorize]
        public async Task<IActionResult> GetHorariosDisponibles(string dateString, long diaHorarioID)
        {
            try
            {
                DateTime date = DateTime.Parse(dateString);
                var horarios = await _workContainer.DiaHorario.GetHorariosDisponibles(date, diaHorarioID);

                return Json(new
                {
                    success = true,
                    data = horarios.Select(x => new
                    {
                        id = x.ID,
                        hora = x.Horario.Hora.ToString("HH:mm"),
                        selected = x.ID == diaHorarioID,
                    }),
                });
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se pudieron encontrar los horarios disponibles", message: "Intente nuevamente o comuníquese para soporte");
            }
        }

        #endregion

        #region Paciente

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateByPaciente")]
        public async Task<IActionResult> CreateByPaciente(Turno turno)
        {
            try
            {
                ModelState.Remove("turno.DiaHorario");
                ModelState.Remove("turno.ObraSocial");
                ModelState.Remove("turno.Persona.ObraSocial");
                if (ModelState.IsValid)
                {
                    Turno turnoConfirmed = await _workContainer.Turno.CreateTurno(turno);

                    var emailError = "";
                    if (turnoConfirmed.Persona.Correo is not null or "")
                    {
                        try
                        {
                            await _workContainer.Email.SendConfirmTurno(turnoConfirmed);
                        }
                        catch (Exception)
                        {
                            emailError = "Sin embargo, no se ha podido enviar el email con el recordatorio";
                        }
                    }

                    return Json(new
                    {
                        success = true,
                        emailError,
                        title = "Su turno se registró correctamente",
                    });
                }
                return CustomBadRequest(title: "No se pudo guardar su turno", message: "Alguno de los datos ingresados no es válido");
            }
            catch (PolicyException e)
            {
                return CustomBadRequest(title: "No se pudo guardar su turno", message: e.Message);
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se pudo guardar su turno", message: "Intente nuevamente o comuníquese telefónicamente");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UpdateByPaciente")]
        public async Task<IActionResult> UpdateByPaciente(Turno turno)
        {
            try
            {
                var oldHorario = turno.DiaHorarioID;

                var newTurno = await _workContainer.Turno.UpdateByPaciente(turno);

                var emailError = "";
                if (newTurno.Persona.Correo is not null or "")
                {
                    try
                    {
                        await _workContainer.Email.SendModifyTurno(turno.ID, oldHorario);
                    }
                    catch (Exception)
                    {
                        emailError = "Sin embargo, no se ha podido enviar el email con el recordatorio";
                    }
                }

                return Json(new
                {
                    success = true,
                    emailError,
                    title = "Su turno se modificó correctamente",
                });
            }
            catch (PolicyException e)
            {
                return CustomBadRequest(title: "No se pudo modificar su turno", message: e.Message);
            }
            catch (Exception)
            {
                return CustomBadRequest(title: "No se pudo modificar su turno", message: "Intente nuevamente o comuníquese telefónicamente");
            }
        }

        [HttpGet]
        [ActionName("GetTurnoByPaciente")]
        public async Task<IActionResult> GetTurnoByPaciente(string nombre, string apellido, string dateString)
        {
            try
            {
                DateTime date = DateTime.Parse(dateString);
                var turno = await _workContainer
                    .Turno
                    .GetTurnoByPaciente(nombre, apellido, date);

                if (turno is null)
                    return CustomBadRequest(title: "No se encontró su turno", message: "Intente nuevamente o comuníquese telefónicamente");

                var horarios = await _workContainer
                    .DiaHorario
                    .GetHorariosDisponibles(turno.DiaHorario.Dia, turno.DiaHorarioID, includeTurno: false);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        turno = new
                        {
                            id = turno.ID,
                            hora = turno.DiaHorario.Horario.Hora.ToString("HH:mm"),
                            dia = turno.DiaHorario.Dia.ToString("dd/MM/yyyy"),
                        },
                        horarios = horarios.Select(x => new
                        {
                            id = x.ID,
                            hora = x.Horario.Hora.ToString("HH:mm"),
                        }),
                    },
                });
            }
            catch (Exception e)
            {
                return CustomBadRequest(title: "No se encontró su turno", message: "Intente nuevamente o comuníquese telefónicamente", error: e.Message);
            }
        }

        #endregion
    }
}
