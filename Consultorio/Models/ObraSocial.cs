using Consultorio.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Consultorio.Models
{
    public class ObraSocial
    {
        [Key]
        public long ID { set; get; }

        [Required(ErrorMessage = "Debes añadir un nombre")]
        public string Nombre { set; get; } = null!;

        [Required(ErrorMessage = "Debes indicar si la obra social está habilitada o no")]
        public bool Habilitada { set; get; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        [MaybeNull]
        public IEnumerable<Paciente> Pacientes { get; set; } = null!;

        [JsonIgnore]
        [MaybeNull]
        public IEnumerable<Persona> Personas { get; set; } = null!;
    }
}
