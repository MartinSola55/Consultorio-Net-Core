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
    public class ObraSocialRepository(ApplicationDbContext db) : Repository<ObraSocial>(db), IObraSocialRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<IEnumerable<SelectListItem>> GetDropDownList()
        {
            IEnumerable<SelectListItem> obrasSociales = new List<SelectListItem>
            {
                new() { Value = "", Text = "Seleccione una obra social", Disabled = true, Selected = true }
            };
            return obrasSociales.Concat(await _db
                .ObraSocial
                .Where(x => x.Habilitada)
                .OrderBy(x => x.Nombre)
                .Select(i => new SelectListItem()
                {
                    Text = i.Nombre,
                    Value = i.ID.ToString(),
                })
                .ToListAsync());
        }

        public async Task SoftDelete(long id)
        {
            var dbObject = await _db
                .ObraSocial
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado la obra social");

            if (await _db.Paciente.AnyAsync(x => x.ObraSocialID == id)) throw new Exception("No se puede eliminar la obra social porque hay pacientes que la tienen asignada");
            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            await _db.SaveChangesAsync();
        }

        public async Task Update(ObraSocial obraSocial)
        {
            var dbObject = await _db
                .ObraSocial
                .FirstAsync(x => x.ID == obraSocial.ID) ?? throw new Exception("No se ha encontrado la obra social");

            dbObject.Nombre = obraSocial.Nombre;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            await _db.SaveChangesAsync();
        }

        public async Task ChangeState(long id)
        {
            var dbObject = await _db
                .ObraSocial
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado la obra social");

            dbObject.Habilitada = !dbObject.Habilitada;
            dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);

            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsDuplicated(ObraSocial obraSocial)
        {
            var dbObject = await _db
                .ObraSocial
                .FirstOrDefaultAsync(x => x.Nombre.ToLower() == obraSocial.Nombre.ToLower() && x.ID != obraSocial.ID);

            return dbObject != null;
        }
    }
}