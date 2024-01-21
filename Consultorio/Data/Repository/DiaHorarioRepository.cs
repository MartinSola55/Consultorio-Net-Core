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

        public void SoftDelete(long id)
        {
            try
            {
                var dbObject = _db.DiaHorario.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado el día");
                dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(DiaHorario diaHorario)
        {
            var dbObject = _db.DiaHorario.First(x => x.ID == diaHorario.ID) ?? throw new Exception("No se ha encontrado el día");
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public List<DiaHorario> GetHorariosByDate(DateTime date)
        {
            List<DiaHorario> dias = [];
            List<DiaHorario> diaHorarios = [.. _db.DiaHorario.Where(x => x.Dia.Date == date.Date).Include(x => x.Horario) ];
            foreach (Horario horario in _db.Horario.ToList())
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

    }
}