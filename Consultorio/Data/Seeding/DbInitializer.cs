using Microsoft.EntityFrameworkCore;
using Consultorio.Models;

namespace Consultorio.Data.Seeding
{
    public class DbInitializer(ModelBuilder modelBuilder)
    {
        private readonly ModelBuilder modelBuilder = modelBuilder;

        public void Seed()
        {
            modelBuilder.Entity<Horario>().HasData(
                new Horario() { ID = 1, Hora = TimeOnly.Parse("09:00") },
                new Horario() { ID = 2, Hora = TimeOnly.Parse("09:20") },
                new Horario() { ID = 3, Hora = TimeOnly.Parse("09:40") },
                new Horario() { ID = 4, Hora = TimeOnly.Parse("10:00") },
                new Horario() { ID = 5, Hora = TimeOnly.Parse("10:20") },
                new Horario() { ID = 6, Hora = TimeOnly.Parse("10:40") },
                new Horario() { ID = 7, Hora = TimeOnly.Parse("11:00") },
                new Horario() { ID = 8, Hora = TimeOnly.Parse("11:20") },
                new Horario() { ID = 9, Hora = TimeOnly.Parse("11:40") },
                new Horario() { ID = 10, Hora = TimeOnly.Parse("12:00") },
                new Horario() { ID = 11, Hora = TimeOnly.Parse("12:20") },
                new Horario() { ID = 12, Hora = TimeOnly.Parse("12:40") },
                new Horario() { ID = 13, Hora = TimeOnly.Parse("13:00") }
            );

        }
    }

}
