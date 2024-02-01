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
    public class DiaHorarioRepository(ApplicationDbContext db) : Repository<DiaHorario>(db), IDiaHorarioRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task SoftDelete(long id)
        {
            var dbObject = await _db
                .DiaHorario
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado el día");

            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            await _db.SaveChangesAsync();
        }

        public async Task<List<DiaHorario>> GetHorariosByDate(DateTime date)
        {
            List<DiaHorario> dias = [];
            List<DiaHorario> diaHorarios = await _db
                .DiaHorario
                .Where(x => x.Dia.Date == date.Date)
                .Include(x => x.Horario)
                .ToListAsync();

            foreach (Horario horario in await _db.Horario.ToListAsync())
            {
                if (diaHorarios.Any(x => x.HorarioID == horario.ID))
                {
                    dias.Add(diaHorarios.First(x => x.HorarioID == horario.ID));
                }
                else
                {
                    dias.Add(new DiaHorario
                    {
                        Horario = horario,
                        Disponible = false,
                    });
                }
            }

            return dias;
        }

        public async Task SaveNew(short[] ids, string dateFrom, string dateTo)
        {
            DateTime dateFromParsed = DateTime.Parse(dateFrom);
            DateTime dateToParsed = DateTime.Parse(dateTo);
            if (dateFromParsed > dateToParsed) throw new Exception("La fecha de inicio no puede ser mayor a la fecha de fin");
            if (ids.Length == 0) throw new Exception("Debe seleccionar al menos un horario");

            List<DiaHorario> dias = [];
            foreach (DateTime date in Enumerable.Range(0, 1 + dateToParsed.Subtract(dateFromParsed).Days).Select(offset => dateFromParsed.AddDays(offset)))
            {
                if (date.DayOfWeek == DayOfWeek.Thursday || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) continue;
                foreach (short id in ids)
                {
                    if (_db.DiaHorario.Any(x => x.Dia.Date == date.Date && x.HorarioID == id)) continue;
                    dias.Add(new DiaHorario
                    {
                        Dia = date,
                        HorarioID = id,
                        Disponible = true,
                    });
                }
            }
            await _db.DiaHorario.AddRangeAsync(dias);
            await _db.SaveChangesAsync();
        }

        public async Task<List<DiaHorario>> GetHorariosDisponibles(DateTime date, long diaHorarioID, bool includeTurno = true)
        {
            var horariosDisponibles = await _db
                .DiaHorario
                .Where(x => x.Dia.Date == date.Date && x.Disponible)
                .Include(x => x.Horario)
                .ToListAsync();

            var diaHorario = await _db
                .DiaHorario
                .Where(x => x.ID == diaHorarioID)
                .Include(x => x.Horario)
                .FirstAsync();

            if (includeTurno && diaHorario is not null && !horariosDisponibles.Contains(diaHorario))
                horariosDisponibles.Add(diaHorario);

            return [.. horariosDisponibles.OrderBy(x => x.Horario.Hora)];
        }
    }
}