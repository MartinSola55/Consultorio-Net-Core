using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Models;

namespace Consultorio.Data.Repository.IRepository
{
    public interface IHorarioRepository : IRepository<Horario>
    {
        List<DiaHorario> GetDisponibles(DateTime date);
    }
}