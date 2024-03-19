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
        Task UpdateDatos(string datoToUpdate, string datoValue, long id);
        Task UpdateHC(HistoriaClinica historiaClinica);
        Task SoftDelete(long id);
        Task<bool> IsDuplicated(Paciente paciente);
        Task<long> AddHC(HistoriaClinica historiaClinica);
        Task DeleteHC(long id);
        Task<List<Paciente>> GetByNacimiento(DateTime nacimiento);
        Task<List<GetByNameResponse>> GetByNombreApellido(string words);
    }
}