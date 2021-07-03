using Microsoft.EntityFrameworkCore;

namespace JSONSource.Models
{
    public class JsonDbContext:DbContext
    {
        public DbSet<Result> Results { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=JSONSourceDb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Result>().HasData(
                new Result() {Id = 1, Temperature = 36.5, Pressure = 200 }
            );

        }
    }
}
