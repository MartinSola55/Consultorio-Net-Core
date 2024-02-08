using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Home
{
    public class IndexViewModel
    {
        public Turno Turno { get; set; } = new Turno();
        public IEnumerable<SelectListItem> ObrasSociales { get; set; } = [];
        public List<Alerta> Alertas = [];
    }
}
