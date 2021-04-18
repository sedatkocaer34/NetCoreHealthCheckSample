using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreHealthCheckSample.Configuration;
using System;

namespace NetCoreHealthCheckSample
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
            services.AddControllers();
            services.AddHealthChecksUI().AddInMemoryStorage();

            var builder = services.AddHealthChecks();
            builder.AddSqlServer(
                Configuration.GetConnectionString("DefaultConnection"),
                healthQuery: "select 1",
                name: "Sql Server",
                failureStatus: null,
                tags: null,
                timeout: TimeSpan.FromSeconds(5));

            // Minimum 1 Gb empty system disk area.

            builder.AddDiskStorageHealthCheck(options => options.AddDrive("C:\\", 1024));

            // Maximum 512 mb system ram area (for virtual).

            builder.AddVirtualMemorySizeHealthCheck(512);

            // Maximum 512 mb system ram area (for private).

            builder.AddPrivateMemoryHealthCheck(512);

            services.AddDbConfiguration(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHealthChecks(path: "/hc",
            options: new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options => 
            {
                options.UIPath = "/hc-ui";
                options.AddCustomStylesheet("Content/health-check-system-ui.css");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
