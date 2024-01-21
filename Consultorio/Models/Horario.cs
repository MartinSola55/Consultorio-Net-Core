using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Consultorio.Models
{
    public class Horario
    {
        [Key]
        [Required(ErrorMessage = "Debes seleccionar una hora")]
        [RegularExpression("^([1-9]|1[0-3])+$", ErrorMessage = "Debes seleccionar una hora válida")]
        public short ID { set; get; }

        [DataType(DataType.Time)]
        public TimeOnly Hora { set; get; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        [NotMapped]
        public string HoraString { get; set; } = null!;
    }
}
