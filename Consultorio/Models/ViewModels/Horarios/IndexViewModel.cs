using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Horarios
{
    public class IndexViewModel
    {
        public IEnumerable<Horario> Horarios { get; set; } = [];
    }
}
