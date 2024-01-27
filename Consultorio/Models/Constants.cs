using System.ComponentModel.DataAnnotations;

namespace Consultorio.Models
{
    public class Constants
    {
        public const string Admin = "ADMIN";
    }

    public class Consultorios
    {
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Ciuidad { get; set; } = "";
        public string Url { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string? Celular { get; set; }
    }
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var displayAttribute = value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                as DisplayAttribute[];

            return displayAttribute[0].Name;
        }
    }
}
