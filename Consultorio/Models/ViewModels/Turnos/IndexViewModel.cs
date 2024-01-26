using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Turnos
{
    public class IndexViewModel
    {
        public IEnumerable<Turno> Turnos { get; set; } = [];
        public IEnumerable<DiaHorario> Horarios { get; set; } = [];
        public IEnumerable<SelectListItem> HorariosDisponibles { get; set; } = [];
        public IEnumerable<SelectListItem> ObrasSociales { get; set; } = [];
        public Turno Turno { get; set; } = new();
    }
}
