using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Models;

namespace Consultorio.Data.Repository.IRepository
{
    public interface IDiaHorarioRepository : IRepository<DiaHorario>
    {
        Task SoftDelete(long id);
        Task<List<DiaHorario>> GetHorariosByDate(DateTime date);
        Task SaveNew(short[] ids, string dateFrom, string dateTo);
        Task<List<DiaHorario>> GetHorariosDisponibles(DateTime date, long diaHorarioID, bool includeTurno = true);
    }
}