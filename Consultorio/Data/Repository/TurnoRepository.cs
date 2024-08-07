using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Consultorio.Data.Repository
{
    public class TurnoRepository(ApplicationDbContext db) : Repository<Turno>(db), ITurnoRepository
    {
        private readonly ApplicationDbContext _db = db;

        private static string ToTitleCase(string text)
        {
            text = Regex.Replace(text, @"[^a-zA-Z\u00C0-\u017F\s]", string.Empty);
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }
        private static string ToNumber(string number)
        {
            number = Regex.Replace(number, @"[^0-9]", string.Empty);
            return number;
        }

        private static bool IsValidHorario(DiaHorario diaHorario)
        {
            var today = DateTime.UtcNow.AddHours(-3);
            var currentHour = today.Hour;
            var currentMinute = today.Minute;

            return !(diaHorario.Dia.Date < today.Date ||
                diaHorario.Dia.Date > today.AddDays(Constants.MaximosDiasReserva).Date ||
                (diaHorario.Dia.Date == today.Date && (diaHorario.Horario.Hora.Hour < currentHour || (diaHorario.Horario.Hora.Hour == currentHour && diaHorario.Horario.Hora.Minute < currentMinute))));
        }

        public async Task SoftDelete(long id)
        {
            var dbObject = await _db
                .Turno
                .Include(x => x.Persona)
                .Include(x => x.DiaHorario)
                .FirstAsync(x => x.DiaHorarioID == id) ?? throw new Exception("No se ha encontrado el turno");

            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.Persona.DeletedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.DiaHorario.Disponible = true;
            dbObject.DiaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            await _db.SaveChangesAsync();
        }

        public async Task<Turno> Update(Turno turno)
        {
            var dbObject = await _db
                .Turno
                .Include(x => x.Persona)
                .Include(x => x.DiaHorario)
                .FirstAsync(x => x.ID == turno.ID) ?? throw new Exception("No se ha encontrado el turno");

            var oldDiaHorario = await _db
                .DiaHorario
                .FirstAsync(x => x.ID == dbObject.DiaHorarioID);

            var newDiaHorario = await _db
                .DiaHorario
                .FirstAsync(x => x.ID == turno.DiaHorarioID);

            oldDiaHorario.Disponible = true;
            oldDiaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            newDiaHorario.Disponible = false;
            newDiaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.Persona.Nombre = ToTitleCase(turno.Persona.Nombre);
            dbObject.Persona.Apellido = ToTitleCase(turno.Persona.Apellido);
            dbObject.Persona.Telefono = ToNumber(turno.Persona.Telefono);
            dbObject.Persona.ObraSocialID = turno.Persona.ObraSocialID;
            dbObject.Persona.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.DiaHorarioID = turno.DiaHorarioID;

            await _db.SaveChangesAsync();
            return await _db.Turno
                .Include(x => x.Persona)
                    .ThenInclude(x => x.ObraSocial)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .FirstAsync(x => x.ID == turno.ID);
        }

        public async Task<List<Turno>> GetByDate(DateTime date)
        {
            return await _db
                .Turno
                .Where(x => x.DiaHorario.Dia.Date == date.Date)
                .Include(x => x.Persona)
                    .ThenInclude(x => x.ObraSocial)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .OrderBy(x => x.DiaHorario.Horario)
                .ToListAsync();
        }

        public async Task<Turno> CreateTurno(Turno turno, bool byPaciente = true)
        {
            if (byPaciente && await CheckDuplicate(turno))
                throw new PolicyException("Ya tienes un turno para ese día");

            var diaHorario = await _db
                .DiaHorario
                .Include(x => x.Horario)
                .FirstAsync(x => x.ID == turno.DiaHorarioID) ?? throw new Exception("No se ha podido validar el turno");

            if (byPaciente && !IsValidHorario(diaHorario))
                throw new PolicyException("El día o el horario del turno no son válidos");

            turno.Persona.Nombre = ToTitleCase(turno.Persona.Nombre);
            turno.Persona.Apellido = ToTitleCase(turno.Persona.Apellido);
            turno.Persona.Telefono = ToNumber(turno.Persona.Telefono);

            await _db.Turno.AddAsync(turno);

            diaHorario.Disponible = false;
            diaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            await _db.SaveChangesAsync();

            return await _db.Turno
                .Include(x => x.Persona)
                    .ThenInclude(x => x.ObraSocial)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .FirstAsync(x => x.ID == turno.ID);
        }

        public async Task<bool> CheckDuplicate(Turno turno)
        {
            var diaHorario = _db.DiaHorario.First(x => x.ID == turno.DiaHorarioID) ?? throw new Exception("No se ha podido validar el turno");
            return await _db
                .Turno
                .AnyAsync(x =>
                x.Persona.Nombre == turno.Persona.Nombre &&
                x.Persona.Apellido == turno.Persona.Apellido &&
                x.DiaHorario.Dia.Date == diaHorario.Dia.Date &&
                x.ID != turno.ID);
        }

        public async Task<Turno?> GetTurnoByPaciente(string nombre, string apellido, DateTime date)
        {
            var today = DateTime.UtcNow.AddHours(-3);
            return await _db
                .Turno
                .Include(x => x.Persona)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .FirstOrDefaultAsync(x =>
                x.Persona.Nombre.ToLower() == nombre.ToLower() &&
                x.Persona.Apellido.ToLower() == apellido.ToLower() &&
                x.DiaHorario.Dia.Date == date.Date &&
                x.DiaHorario.Dia.Date >= today.Date &&
                x.DiaHorario.Dia.Date <= today.AddDays(Constants.MaximosDiasReserva).Date);
        }

        public async Task<Turno> UpdateByPaciente(Turno turno)
        {
            var dbObject = await _db.Turno
                .Include(x => x.Persona)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .FirstAsync(x => x.ID == turno.ID) ?? throw new Exception("No se ha encontrado el turno");

            turno.Persona = dbObject.Persona;
            if (await CheckDuplicate(turno))
                throw new PolicyException("Ya tienes un turno para ese día");

            var diaHorario = await _db
                .DiaHorario
                .Include(x => x.Horario)
                .FirstAsync(x => x.ID == turno.DiaHorarioID) ?? throw new Exception("No se ha podido validar el turno");

            if (!diaHorario.Disponible)
                throw new PolicyException("El turno ya no está disponible");
            if (!IsValidHorario(diaHorario))
                throw new PolicyException("La fecha del turno no es válida");

            diaHorario.Disponible = false;
            diaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.DiaHorarioID = turno.DiaHorarioID;
            dbObject.DiaHorario.Disponible = true;
            dbObject.DiaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            await _db.SaveChangesAsync();

            return dbObject;
        }
    }
}