using JsonSource.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JsonSource.Services;
using Microsoft.EntityFrameworkCore;

namespace JsonSource
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MyAppSettings>(Configuration.GetSection(MyAppSettings.SectionName));
            services.AddDbContext<JsonDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("JsonConnectionString")));
            services.AddControllers();

            services.AddTransient<IResultsService, ResultsService>();
            services.AddTransient<IKeyManager, KeyManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.ApplyMigrations();
        }
    }
}
