using Microsoft.EntityFrameworkCore;

namespace XMLSource.Models
{
    public class XMLDbContext:DbContext
    {
        public XMLDbContext(DbContextOptions<XMLDbContext> options)
            :base(options)
        {

        }
        public DbSet<Result> Results { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Result>().HasData(
                new Result() { Id = 1, Temperature = 20.5, Pressure = 916 }
            );

        }
    }
}
