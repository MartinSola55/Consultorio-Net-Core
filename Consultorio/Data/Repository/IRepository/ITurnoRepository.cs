using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Models;

namespace Consultorio.Data.Repository.IRepository
{
    public interface ITurnoRepository : IRepository<Turno>
    {
        // GetAwaiter



        Task<Turno> Update(Turno turno);
        Task<Turno> UpdateByPaciente(Turno turno);
        Task SoftDelete(long id);
        Task<List<Turno>> GetByDate(DateTime date);
        Task<Turno> CreateTurno(Turno turno, bool byPaciente = true);
        Task<bool> CheckDuplicate(Turno turno);
        Task<Turno?> GetTurnoByPaciente(string nombre, string apellido, DateTime date);
    }
}