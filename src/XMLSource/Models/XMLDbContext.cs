using Microsoft.EntityFrameworkCore;

namespace XmlSource.Models
{
    public class XmlDbContext:DbContext
    {
        public XmlDbContext(DbContextOptions<XmlDbContext> options)
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
