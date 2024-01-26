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

        public void Update(Turno turno)
        {
            var dbObject = _db.Turno.First(x => x.ID == turno.ID) ?? throw new Exception("No se ha encontrado el turno");
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
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
    }
}