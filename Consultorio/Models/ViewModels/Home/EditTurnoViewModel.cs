using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio.Models.ViewModels.Home
{
    public class EditTurnoViewModel
    {
        public Turno Turno { get; set; } = new();
    }
}
