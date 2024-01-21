using Consultorio.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Consultorio.Models
{
    public class Turno
    {
        [Key]
        public long ID { set; get; }

        [Required(ErrorMessage = "Debes añadir una persona")]
        public long PersonaID { set; get; }

        [Required(ErrorMessage = "Debes añadir un día y un horario")]
        public long DiaHorarioID { set; get; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual Persona Persona { set; get; } = null!;

        public virtual DiaHorario DiaHorario { set; get; } = null!;
    }
}
