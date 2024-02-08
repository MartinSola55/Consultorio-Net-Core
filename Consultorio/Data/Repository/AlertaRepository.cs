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
    public class AlertaRepository(ApplicationDbContext db) : Repository<Alerta>(db), IAlertaRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<List<Alerta>> GetActivas()
        {
            return await _db
                .Alerta
                .Where(x => x.Desde.Date <= DateTime.UtcNow.AddHours(-3).Date && x.Hasta.Date >= DateTime.UtcNow.AddHours(-3).Date)
                .ToListAsync();
        }

        public async Task SoftDelete(long id)
        {
            var alerta = await _db
                .Alerta
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado la alerta");

            alerta.DeletedAt = DateTime.UtcNow.AddHours(-3);
            await _db.SaveChangesAsync();
        }
    }
}