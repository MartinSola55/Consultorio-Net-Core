using Consultorio.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Consultorio.Models
{
    public class Paciente
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

        [Required(ErrorMessage = "Debes seleccionar una obra social")]
        [Display(Name = "Obra Social")]
        public long ObraSocialID { set; get; }

        [Required(ErrorMessage = "Debes seleccionar una fecha de nacimiento")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Debes añadir una dirección")]
        [StringLength(50, ErrorMessage = "Debes añadir una dirección de menos de 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z\u00C0-\u017F\s0-9.]+$", ErrorMessage = "Ingrese una dirección válida")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "Debes añadir una localidad")]
        [StringLength(30, ErrorMessage = "Debes añadir una localidad de menos de 30 caracteres")]
        [RegularExpression(@"^[a-zA-Z\u00C0-\u017F\s]+$", ErrorMessage = "Ingrese una localidad válida")]
        public string Localidad { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public IEnumerable<HistoriaClinica> HistoriasClinicas { get; set; } = null!;

        public virtual ObraSocial ObraSocial { set; get; } = null!;
    }
}
