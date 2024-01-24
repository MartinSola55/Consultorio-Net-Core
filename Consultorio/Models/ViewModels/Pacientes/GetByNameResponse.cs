using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Pacientes
{
    public class GetByNameResponse
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Localidad { get; set; } = "";
        public string ObraSocial { get; set; } = "";
        public string FechaNacimiento { get; set; } = "";
        public string UpdatedAt { get; set; } = "";
    }
}
