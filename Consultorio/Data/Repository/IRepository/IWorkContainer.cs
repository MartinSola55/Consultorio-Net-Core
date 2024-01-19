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

        void BeginTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}