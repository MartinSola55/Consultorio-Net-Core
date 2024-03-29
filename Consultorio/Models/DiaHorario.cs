﻿using Consultorio.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Consultorio.Models
{
    public class DiaHorario
    {
        [Key]
        public long ID { set; get; }

        [Required(ErrorMessage = "Debes seleccionar un horario")]
        public short HorarioID { set; get; }

        [Required(ErrorMessage = "Debes seleccionar un día")]
        [DataType(DataType.Date)]
        public DateTime Dia { set; get; }

        [DefaultValue(true)]
        public bool Disponible { set; get; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);

        public DateTime? DeletedAt { get; set; }

        public virtual Horario Horario { set; get; } = null!;
    }
}
