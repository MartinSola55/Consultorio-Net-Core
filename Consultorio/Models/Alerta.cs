using Consultorio.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Consultorio.Models
{
    public class Alerta
    {
        [Key]
        public long ID { set; get; }

        [Required(ErrorMessage = "Debes seleccionar un día desde")]
        [DataType(DataType.Date)]
        public DateTime Desde { set; get; }

        [Required(ErrorMessage = "Debes seleccionar un día hasta")]
        [DataType(DataType.Date)]
        public DateTime Hasta { set; get; }

        public string Mensaje { set; get; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }
    }
}
