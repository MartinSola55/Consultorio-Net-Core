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
        Turno Update(Turno turno);
        void UpdateByPaciente(Turno turno);
        void SoftDelete(long id);
        List<Turno> GetByDate(DateTime date);
        Turno CreateTurno(Turno turno, bool byPaciente = true);
        bool CheckDuplicate(Turno turno);
        Turno? GetTurnoByPaciente(string nombre, string apellido, DateTime date);
    }
}