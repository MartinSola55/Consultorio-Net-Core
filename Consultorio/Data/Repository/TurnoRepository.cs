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
    public class TurnoRepository(ApplicationDbContext db) : Repository<Turno>(db), ITurnoRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void SoftDelete(long id)
        {
            try
            {
                var dbObject = _db.Turno.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado el turno");
                dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(Turno turno)
        {
            var dbObject = _db.Turno.First(x => x.ID == turno.ID) ?? throw new Exception("No se ha encontrado el turno");
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

    }
}