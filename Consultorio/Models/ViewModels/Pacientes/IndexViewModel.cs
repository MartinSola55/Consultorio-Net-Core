using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Pacientes
{
    public class IndexViewModel
    {
        public IEnumerable<Paciente> Pacientes { get; set; } = [];
        public IEnumerable<SelectListItem> ObrasSociales { get; set; } = [];
        public Paciente Paciente { get; set; } = new();
    }
}
