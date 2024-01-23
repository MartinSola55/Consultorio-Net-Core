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
                dbObject.HistoriasClinicas.ToList().ForEach(x => x.DeletedAt = DateTime.UtcNow.AddHours(-3));
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
            var paciente = _db.Paciente.FirstOrDefault(x => x.ID == historiaClinica.PacienteID) ?? throw new Exception("No se ha encontrado el paciente");
            paciente.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            _db.HistoriaClinica.Add(historiaClinica);
            _db.SaveChanges();
            return historiaClinica.ID;
        }

        public void UpdateDatos(string datoToUpdate, string datoValue, long id)
        {
            Paciente paciente = _db.Paciente.FirstOrDefault(x => x.ID == id) ?? throw new Exception("No se ha encontrado el paciente");

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
            _db.SaveChanges();
        }

        public void UpdateHC(HistoriaClinica historiaClinica)
        {
            var hc = _db.HistoriaClinica.FirstOrDefault(x => x.ID == historiaClinica.ID) ?? throw new Exception("No se ha encontrado la historia clínica");
            var paciente = _db.Paciente.FirstOrDefault(x => x.ID == hc.PacienteID) ?? throw new Exception("No se ha encontrado el paciente");
            hc.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            paciente.UpdatedAt = DateTime.UtcNow.AddHours(-3);
            hc.Descripcion = historiaClinica.Descripcion;
            hc.Fecha = historiaClinica.Fecha;
            _db.SaveChanges();
        }
    }
}