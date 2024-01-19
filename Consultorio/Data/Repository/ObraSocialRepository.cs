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
    public class ObraSocialRepository(ApplicationDbContext db) : Repository<ObraSocial>(db), IObraSocialRepository
    {
        private readonly ApplicationDbContext _db = db;

        public IEnumerable<SelectListItem> GetDropDownList()
        {
            IEnumerable<SelectListItem> obrasSociales = new List<SelectListItem>
            {
                new() { Value = "", Text = "Seleccione un usuario", Disabled = true, Selected = true }
            };
            return obrasSociales.Concat(_db.ObraSocial.Where(x => x.Habilitada).OrderBy(x => x.Nombre).Select(i => new SelectListItem()
            {
                Text = i.Nombre,
                Value = i.ID.ToString(),
            }));
        }

        public void SoftDelete(long id)
        {
            try
            {
                var dbObject = _db.ObraSocial.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado la obra social");
                dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(ObraSocial obraSocial)
        {
            var dbObject = _db.ObraSocial.First(x => x.ID == obraSocial.ID) ?? throw new Exception("No se ha encontrado la obra social");
            dbObject.Nombre = obraSocial.Nombre;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public void ChangeState(long id)
        {
            var dbObject = _db.ObraSocial.First(x => x.ID == id) ?? throw new Exception("No se ha encontrado la obra social");
            dbObject.Habilitada = !dbObject.Habilitada;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.SaveChanges();
        }

        public bool IsDuplicated(ObraSocial obraSocial)
        {
            var dbObject = _db.ObraSocial.FirstOrDefault(x => x.Nombre.ToLower() == obraSocial.Nombre.ToLower() && x.ID != obraSocial.ID);
            return dbObject != null;
        }
    }
}