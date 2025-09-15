using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Home
{
    public class ContactoViewModel
    {
        public List<Consultorios> Consultorios { get; set; } = new()
        {
            new Consultorios
            {
                Codigo = "SCC",
                Nombre = "Consultorio particular",
                Ciuidad = "San Carlos Centro",
                Direccion = "Almafuerte 384, San Carlos Centro",
                Celular = "5493404520532",
                Url = "https://maps.app.goo.gl/cBzwrkopGtQCWQzj6",
            },
            new Consultorios
            {
                Codigo = "SJN",
                Nombre = "Sanatorio San Jerónimo Norte",
                Ciuidad = "San Jerónimo Norte",
                Direccion = "1 de Mayo 411, San Jeronimo Norte",
                Telefono = "03404-460397",
                Url = "https://maps.app.goo.gl/rRRJ39s6BYfsq5fD9",
            },
            new Consultorios
            {
                Codigo = "GAL",
                Nombre = "Grupo Sigma Salud",
                Ciuidad = "Gálvez",
                Direccion = "R. F. Coulin 642, Gálvez",
                Telefono = "03404-431700",
                Url = "https://maps.app.goo.gl/kdggK7svSk1ovdcd9",
            }
        };
        
    }
}
