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

namespace Consultorio.Data.Repository
{
    public class TurnoRepository(ApplicationDbContext db) : Repository<Turno>(db), ITurnoRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void SoftDelete(long id)
        {
            var dbObject = _db.Turno.Include(x => x.Persona).Include(x => x.DiaHorario).First(x => x.DiaHorarioID == id) ?? throw new Exception("No se ha encontrado el turno");
            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.Persona.DeletedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.DiaHorario.Disponible = true;
            dbObject.DiaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public Turno Update(Turno turno)
        {
            var dbObject = _db.Turno.Include(x => x.Persona).Include(x => x.DiaHorario).First(x => x.ID == turno.ID) ?? throw new Exception("No se ha encontrado el turno");
            var oldDiaHorario = _db.DiaHorario.First(x => x.ID == dbObject.DiaHorarioID);
            var newDiaHorario = _db.DiaHorario.First(x => x.ID == turno.DiaHorarioID);
            oldDiaHorario.Disponible = true;
            oldDiaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            newDiaHorario.Disponible = false;
            newDiaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.Persona.Nombre = turno.Persona.Nombre;
            dbObject.Persona.Apellido = turno.Persona.Apellido;
            dbObject.Persona.Telefono = turno.Persona.Telefono;
            dbObject.Persona.ObraSocialID = turno.Persona.ObraSocialID;
            dbObject.Persona.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.DiaHorarioID = turno.DiaHorarioID;
            _db.SaveChanges();
            return _db.Turno
                .Include(x => x.Persona)
                    .ThenInclude(x => x.ObraSocial)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .First(x => x.ID == turno.ID);
        }

        public List<Turno> GetByDate(DateTime date)
        {
            return [
                ..
                _db.Turno.Where(x => x.DiaHorario.Dia.Date == date.Date)
                .Include(x => x.Persona)
                    .ThenInclude(x => x.ObraSocial)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .OrderBy(x => x.DiaHorario.Horario)
                .ToList()
            ];
        }

        public async Task<Turno> CreateTurno(Turno turno, bool byPaciente = true)
        {
            if (byPaciente && CheckDuplicate(turno))
                throw new PolicyException("Ya tienes un turno para ese día");

            await _db.Turno.AddAsync(turno);
            var diaHorario = _db.DiaHorario.First(x => x.ID == turno.DiaHorarioID);
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

        public bool CheckDuplicate(Turno turno)
        {
            var diaHorario = _db.DiaHorario.First(x => x.ID == turno.DiaHorarioID) ?? throw new Exception("No se ha podido validar el turno");
            return _db.Turno.Any(
                x => x.Persona.Nombre == turno.Persona.Nombre &&
                x.Persona.Apellido == turno.Persona.Apellido &&
                x.DiaHorario.Dia.Date == diaHorario.Dia.Date &&
                x.ID != turno.ID);
        }

        public Turno? GetTurnoByPaciente(string nombre, string apellido, DateTime date)
        {
            var today = DateTime.UtcNow.AddHours(-3);
            return _db.Turno
                .Include(x => x.Persona)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .FirstOrDefault(x =>
                x.Persona.Nombre == nombre &&
                x.Persona.Apellido == apellido &&
                x.DiaHorario.Dia.Date == date.Date &&
                x.DiaHorario.Dia.Date > today.Date &&
                x.DiaHorario.Dia.Date <= today.AddDays(Constants.MaximosDiasReserva).Date);
        }

        public void UpdateByPaciente(Turno turno)
        {
            var dbObject = _db.Turno.Include(x => x.Persona).Include(x => x.DiaHorario).First(x => x.ID == turno.ID) ?? throw new Exception("No se ha encontrado el turno");
            turno.Persona = dbObject.Persona;
            if (CheckDuplicate(turno))
                throw new PolicyException("Ya tienes un turno para ese día");

            var diaHorario = _db.DiaHorario.First(x => x.ID == turno.DiaHorarioID) ?? throw new Exception("No se ha podido validar el turno");

            if (!diaHorario.Disponible)
                throw new PolicyException("El turno ya no está disponible");
            var today = DateTime.UtcNow.AddHours(-3);
            if (diaHorario.Dia.Date < today.AddDays(1).Date || diaHorario.Dia.Date > today.AddDays(Constants.MaximosDiasReserva).Date)
                throw new PolicyException("El cambio debe ser al menos con un día de anticipación");

            diaHorario.Disponible = false;
            diaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.DiaHorarioID = turno.DiaHorarioID;
            dbObject.DiaHorario.Disponible = true;
            dbObject.DiaHorario.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }
    }
}