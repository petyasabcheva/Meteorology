using Microsoft.EntityFrameworkCore;

namespace JSONSource.Models
{
    public class JsonDbContext:DbContext
    {
        public JsonDbContext(DbContextOptions<JsonDbContext> options)
            : base(options)
        {
               
        }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Result>().HasData(
                new Result() {Id = 1, Temperature = 36.5, Pressure = 1000 }
            );

        }
    }
}
