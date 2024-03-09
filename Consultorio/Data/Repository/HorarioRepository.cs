using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Microsoft.EntityFrameworkCore;

namespace Consultorio.Data.Repository
{
    public class HorarioRepository(ApplicationDbContext db) : Repository<Horario>(db), IHorarioRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<List<DiaHorario>> GetDisponibles(DateTime date)
        {
            var today = DateTime.UtcNow.AddHours(-3);
            var currentHour = today.Hour;
            var currentMinute = today.Minute;

            return await _db
                .DiaHorario
                .Include(x => x.Horario)
                .Where(x => x.Dia.Date == date.Date && x.Disponible && (x.Horario.Hora.Hour > currentHour || (x.Horario.Hora.Hour == currentHour && x.Horario.Hora.Minute > currentMinute)))
                .OrderBy(x => x.Horario.Hora)
                .ToListAsync();
        }
    }
}