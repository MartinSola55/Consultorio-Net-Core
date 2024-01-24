using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Models;
using Consultorio.Models.ViewModels.Pacientes;

namespace Consultorio.Data.Repository.IRepository
{
    public interface IPacienteRepository : IRepository<Paciente>
    {
        void Update(Paciente paciente);
        void UpdateDatos(string datoToUpdate, string datoValue, long id);
        void UpdateHC(HistoriaClinica historiaClinica);
        void SoftDelete(long id);
        bool IsDuplicated(Paciente paciente);
        long AddHC(HistoriaClinica historiaClinica);
        List<Paciente> GetByNacimiento(DateTime nacimiento);
        Task<List<GetByNameResponse>> GetByNombreApellido(string words);
    }
}