using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Pacientes
{
    public class DetallesViewModel
    {
        public Paciente Paciente { get; set; } = new();
        public IEnumerable<SelectListItem> ObraSociales { get; set; } = [];
    }
}
