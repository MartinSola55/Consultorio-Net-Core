using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;

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
            throw new NotImplementedException();
        }

    }
}