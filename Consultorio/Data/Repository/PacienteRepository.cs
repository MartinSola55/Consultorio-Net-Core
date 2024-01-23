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
    public class PacienteRepository(ApplicationDbContext db) : Repository<Paciente>(db), IPacienteRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void SoftDelete(long id)
        {
            try
            {
                var dbObject = _db.Paciente.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado el paciente");
                dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(Paciente paciente)
        {
            var dbObject = _db.Paciente.First(x => x.ID == paciente.ID) ?? throw new Exception("No se ha encontrado la obra social");
            _db.SaveChanges();
        }

        public bool IsDuplicated(Paciente paciente)
        {
            var dbObject = _db.Paciente.FirstOrDefault(x => x.Nombre.ToLower() == paciente.Nombre.ToLower() && x.ID != paciente.ID);
            return dbObject != null;
        }

        public long AddHC(HistoriaClinica historiaClinica)
        {
            _db.HistoriaClinica.Add(historiaClinica);
            _db.SaveChanges();
            return historiaClinica.ID;
        }
    }
}