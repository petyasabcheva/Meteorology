using Microsoft.EntityFrameworkCore;

namespace XMLSource.Models
{
    public class XMLDbContext:DbContext
    {
        public DbSet<Result> Results { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=XMLSourceDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Result>().HasData(
                new Result() { Id = 1, Temperature = 20.5, Pressure = 108 }
            );

        }
    }
}
