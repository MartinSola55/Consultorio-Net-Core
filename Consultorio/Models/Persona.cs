using Consultorio.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Consultorio.Models
{
    public class Persona
    {
        [Key]
        public long ID { set; get; }

        [Required(ErrorMessage = "Debes añadir un nombre")]
        [StringLength(30, ErrorMessage = "Debes añadir un nombre de menos de 30 caracteres")]
        [RegularExpression(@"^[a-zA-Z\u00C0-\u017F\s]+$", ErrorMessage = "Ingrese un nombre válido")]
        public string Nombre { set; get; } = null!;

        [Required(ErrorMessage = "Debes añadir un apellido")]
        [StringLength(30, ErrorMessage = "Debes añadir un apellido de menos de 30 caracteres")]
        [RegularExpression(@"^[a-zA-Z\u00C0-\u017F\s']+$", ErrorMessage = "Ingrese un apellido válido")]
        public string Apellido { set; get; } = null!;

        [Required(ErrorMessage = "Debes añadir un teléfono")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Debes añadir un teléfono válido")]
        public string Telefono { set; get; } = null!;

        [EmailAddress(ErrorMessage = "Debes ingresar un formato de email válido")]
        public string Correo { set; get; } = null!;

        [Required(ErrorMessage = "Debes seleccionar una obra social")]
        public long ObraSocialID { set; get; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual ObraSocial ObraSocial { set; get; } = null!;
    }
}
