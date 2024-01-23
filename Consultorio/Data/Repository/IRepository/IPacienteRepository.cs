using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Models;

namespace Consultorio.Data.Repository.IRepository
{
    public interface IPacienteRepository : IRepository<Paciente>
    {
        void Update(Paciente paciente);
        void UpdateDatos(string datoToUpdate, string datoValue, long id);
        void SoftDelete(long id);
        bool IsDuplicated(Paciente paciente);
        long AddHC(HistoriaClinica historiaClinica);
    }
}