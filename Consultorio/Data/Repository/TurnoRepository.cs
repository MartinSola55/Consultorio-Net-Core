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
    public class TurnoRepository(ApplicationDbContext db) : Repository<Turno>(db), ITurnoRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void SoftDelete(long id)
        {
            var dbObject = _db.Turno.Include(x => x.Persona).Include(x => x.DiaHorario).First(x => x.DiaHorarioID == id) ?? throw new Exception("No se ha encontrado el turno");
            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.Persona.DeletedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.DiaHorario.Disponible = true;
            _db.SaveChanges();
        }

        public Turno Update(Turno turno)
        {
            var dbObject = _db.Turno.Include(x => x.Persona).Include(x => x.DiaHorario).First(x => x.ID == turno.ID) ?? throw new Exception("No se ha encontrado el turno");
            var oldDiaHorario = _db.DiaHorario.First(x => x.ID == dbObject.DiaHorarioID);
            var newDiaHorario = _db.DiaHorario.First(x => x.ID == turno.DiaHorarioID);
            oldDiaHorario.Disponible = true;
            newDiaHorario.Disponible = false;
            
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.Persona.Nombre = turno.Persona.Nombre;
            dbObject.Persona.Apellido = turno.Persona.Apellido;
            dbObject.Persona.Telefono = turno.Persona.Telefono;
            dbObject.Persona.ObraSocialID = turno.Persona.ObraSocialID;
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

        public Turno CreateTurno(Turno turno)
        {
            _db.Turno.Add(turno);
            var diaHorario = _db.DiaHorario.First(x => x.ID == turno.DiaHorarioID);
            diaHorario.Disponible = false;
            _db.SaveChanges();
            return _db.Turno
                .Include(x => x.Persona)
                    .ThenInclude(x => x.ObraSocial)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .First(x => x.ID == turno.ID);
        }

        public bool CheckDuplicate(Turno turno)
        {
            var diaHorario = _db.DiaHorario.First(x => x.ID == turno.DiaHorarioID) ?? throw new Exception("No se ha podido validar el turno");
            return _db.Turno.Any(
                x => x.Persona.Nombre == turno.Persona.Nombre &&
                x.Persona.Apellido == turno.Persona.Apellido &&
                x.DiaHorario.Dia.Date == diaHorario.Dia.Date);
        }

        public Turno? GetTurnoByPaciente(string nombre, string apellido, DateTime date)
        {
            var today = DateTime.UtcNow.AddHours(-3);
            return _db.Turno
                .Include(x => x.Persona)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .FirstOrDefault(x => x.Persona.Nombre == nombre && x.Persona.Apellido == apellido && x.DiaHorario.Dia.Date == date.Date && x.DiaHorario.Dia.Date > today.Date);
        }
    }
}