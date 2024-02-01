using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultorio.Data.Repository.IRepository
{
    public interface IWorkContainer : IDisposable
    {
        // Agregar los repositorios
        IApplicationUserRepository ApplicationUser { get; }
        IObraSocialRepository ObraSocial { get; }
        ITurnoRepository Turno { get; }
        IDiaHorarioRepository DiaHorario { get; }
        IHorarioRepository Horario { get; }
        IPacienteRepository Paciente { get; }
        IEmailRepository Email { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
        void Save();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveAsync();
    }
}