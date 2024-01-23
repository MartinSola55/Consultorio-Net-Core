using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Pacientes
{
    public class IndexViewModel
    {
        public IEnumerable<Paciente> Pacientes { get; set; } = [];
    }
}
