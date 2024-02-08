using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Alertas
{
    public class IndexViewModel
    {
        public List<Alerta> Alertas { get; set; } = [];
        public Alerta Alerta { get; set; } = new();
    }
}
