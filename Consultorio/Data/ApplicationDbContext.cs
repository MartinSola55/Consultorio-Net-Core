using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Consultorio.Data.Seeding;
using Consultorio.Models;

namespace Consultorio.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales aquí

            new DbInitializer(modelBuilder).Seed();

            modelBuilder.Entity<DiaHorario>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<HistoriaClinica>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<Horario>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<ObraSocial>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<Paciente>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<Persona>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<Turno>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<Alerta>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
        }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<DiaHorario> DiaHorario { get; set; }
        public DbSet<HistoriaClinica> HistoriaClinica { get; set; }
        public DbSet<Horario> Horario { get; set; }
        public DbSet<ObraSocial> ObraSocial { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Turno> Turno { get; set; }
        public DbSet<Alerta> Alerta { get; set; }
    
    }
}