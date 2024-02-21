using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Consultorio.Models.ViewModels.Pacientes;

namespace Consultorio.Data.Repository
{
    public class PacienteRepository(ApplicationDbContext db) : Repository<Paciente>(db), IPacienteRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task SoftDelete(long id)
        {
            var dbObject = await _db
                .Paciente
                .Include(x => x.HistoriasClinicas)
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado el paciente");

            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            dbObject.HistoriasClinicas.ToList().ForEach(x => x.DeletedAt = DateTime.UtcNow.AddHours(-3));

            await _db.SaveChangesAsync();
        }

        public async Task Update(Paciente paciente)
        {
            var dbObject = await _db
                .Paciente
                .FirstAsync(x => x.ID == paciente.ID) ?? throw new Exception("No se ha encontrado la obra social");

            await _db.SaveChangesAsync();
        }

        public Task<bool> IsDuplicated(Paciente paciente)
        {
            var dbObject = _db
                .Paciente
                .AnyAsync(x =>
                    x.Nombre.ToLower() == paciente.Nombre.ToLower() &&
                    x.Apellido.ToLower() == paciente.Apellido.ToLower() &&
                    x.FechaNacimiento.Date == paciente.FechaNacimiento.Date &&
                    x.ID != paciente.ID);

            return dbObject;
        }

        public async Task<long> AddHC(HistoriaClinica historiaClinica)
        {
            var paciente = await _db
                .Paciente
                .FirstAsync(x => x.ID == historiaClinica.PacienteID) ?? throw new Exception("No se ha encontrado el paciente");

            paciente.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            await _db.HistoriaClinica.AddAsync(historiaClinica);
            await _db.SaveChangesAsync();
            return historiaClinica.ID;
        }

        public async Task UpdateDatos(string datoToUpdate, string datoValue, long id)
        {
            Paciente paciente = await _db
                .Paciente
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado el paciente");

            switch (datoToUpdate)
            {
                case "Nombre":
                    paciente.Nombre = datoValue;
                    break;
                case "Apellido":
                    paciente.Apellido = datoValue;
                    break;
                case "FechaNacimiento":
                    paciente.FechaNacimiento = DateTime.Parse(datoValue);
                    break;
                case "Telefono":
                    paciente.Telefono = datoValue;
                    break;
                case "ObraSocial":
                    paciente.ObraSocialID = long.Parse(datoValue);
                    break;
                case "Direccion":
                    paciente.Direccion = datoValue;
                    break;
                case "Localidad":
                    paciente.Localidad = datoValue;
                    break;
                default:
                    throw new Exception("No se ha encontrado el dato a actualizar");
            }
            paciente.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateHC(HistoriaClinica historiaClinica)
        {
            var hc = await _db
                .HistoriaClinica
                .FirstAsync(x => x.ID == historiaClinica.ID) ?? throw new Exception("No se ha encontrado la historia clínica");

            var paciente = await _db
                .Paciente
                .FirstAsync(x => x.ID == hc.PacienteID) ?? throw new Exception("No se ha encontrado el paciente");

            paciente.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            hc.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            hc.Descripcion = historiaClinica.Descripcion;
            hc.Fecha = historiaClinica.Fecha;
            await _db.SaveChangesAsync();
        }

        public async Task<List<Paciente>> GetByNacimiento(DateTime nacimiento)
        {
            return await _db
                .Paciente
                .Where(x => x.FechaNacimiento.Date == nacimiento.Date)
                .Include(x => x.ObraSocial)
                .OrderBy(x => x.Apellido)
                    .ThenBy(x => x.Nombre)
                .ToListAsync();
        }

        public async Task<List<GetByNameResponse>> GetByNombreApellido(string words)
        {
            return await _db.Paciente
                .Where(x => (x.Nombre + " " + x.Apellido).Contains(words) || (x.Apellido + " " + x.Nombre).Contains(words))
                .Include(x => x.ObraSocial)
                .Select(x => new GetByNameResponse
                {
                    Id = x.ID,
                    Nombre = x.Nombre,
                    Apellido = x.Apellido,
                    Telefono = x.Telefono ?? "-",
                    Direccion = x.Direccion ?? "-",
                    Localidad = x.Localidad ?? "-",
                    FechaNacimiento = x.FechaNacimiento.ToString("dd/MM/yyyy"),
                    ObraSocial = x.ObraSocial.Nombre,
                    UpdatedAt = x.UpdatedAt.ToString("dd/MM/yyyy")
                })
                .OrderBy(x => x.Apellido)
                .ThenBy(x => x.Nombre)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task DeleteHC(long id)
        {
            var dbObject = await _db
                .HistoriaClinica
                .FirstAsync(x => x.ID == id) ?? throw new Exception("No se ha encontrado el paciente");

            dbObject.DeletedAt = DateTime.UtcNow.AddHours(-3);
            await _db.SaveChangesAsync();
        }
    }
}