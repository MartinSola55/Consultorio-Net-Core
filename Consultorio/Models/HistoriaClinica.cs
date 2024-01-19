using Consultorio.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Consultorio.Models
{
    public class HistoriaClinica
    {
        [Key]
        public long ID { set; get; }

        [Required(ErrorMessage = "Debes seleccionar un paciente")]
        [Display(Name = "Paciente")]
        public long PacienteID { get; set; }

        [Required(ErrorMessage = "Debes ingresar una descripción")]
        [StringLength(1500, ErrorMessage = "Debes añadir una descripción de menos de 1500 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar una fecha")]
        public DateTime Fecha { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual Paciente Paciente { set; get; } = null!;
    }
}
