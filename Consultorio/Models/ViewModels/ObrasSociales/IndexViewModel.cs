using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.ObrasSociales
{
    public class IndexViewModel
    {
        public IEnumerable<ObraSocial> ObraSociales { get; set; } = [];
        public ObraSocial NewObraSocial { get; set; } = new();
    }
}
