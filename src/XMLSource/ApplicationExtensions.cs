using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XmlSource.Models;

namespace XmlSource
{
    public static class ApplicationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();

            var dbContext = services.ServiceProvider.GetService<XmlDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
