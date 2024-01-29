using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Models;
using Consultorio.Models.ViewModels.Pacientes;

namespace Consultorio.Data.Repository.IRepository
{
    public interface IEmailRepository
    {
        Task SendConfirmTurno(Turno turno);
        Task SendModifyTurno(long turnoID, long oldHorarioID);
    }
}