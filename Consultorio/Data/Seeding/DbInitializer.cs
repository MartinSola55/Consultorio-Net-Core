using Microsoft.EntityFrameworkCore;
using Consultorio.Models;

namespace Consultorio.Data.Seeding
{
    public class DbInitializer(ModelBuilder modelBuilder)
    {
        private readonly ModelBuilder modelBuilder = modelBuilder;

        public void Seed()
        {
            
        }
    }

}
