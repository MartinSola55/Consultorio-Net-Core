using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consultorio.Data.Repository.IRepository;

namespace Consultorio.Data.Repository
{
    public class WorkContainer : IWorkContainer
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public WorkContainer(ApplicationDbContext db, IConfiguration config, IWebHostEnvironment env)
        {
            _db = db;
            _config = config;
            _env = env;
            ApplicationUser = new ApplicationUserRepository(_db);
            ObraSocial = new ObraSocialRepository(_db);
            Turno = new TurnoRepository(_db);
            DiaHorario = new DiaHorarioRepository(_db);
            Horario = new HorarioRepository(_db);
            Paciente = new PacienteRepository(_db);
            Email = new EmailRepository(_config, _env);
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IObraSocialRepository ObraSocial { get; private set; }
        public ITurnoRepository Turno { get; private set; }
        public IDiaHorarioRepository DiaHorario { get; private set; }
        public IHorarioRepository Horario { get; private set; }
        public IPacienteRepository Paciente { get; private set; }
        public IEmailRepository Email { get; private set; }

        public void BeginTransaction()
        {
            _db.Database.BeginTransaction();
        }

        public void Commit()
        {
            _db.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _db.Database.RollbackTransaction();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}