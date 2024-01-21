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
                IEnumerable<Turno> turnos = _workContainer.Turno.GetAll(x => x.DiaHorario.Dia.Date == date.Date, includeProperties: "Persona, DiaHorario, Persona.ObraSocial, DiaHorario.Horario").OrderBy(x => x.DiaHorario.Horario);
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
                            disponible = diaHorario.Disponible
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
    }
}
