using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

            builder.AddRedis(redisConnectionString: Configuration.GetConnectionString("Redis"),
            failureStatus: HealthStatus.Degraded);

            // Minimum 1 Gb empty system disk area.
            builder.AddDiskStorageHealthCheck(options => options.AddDrive("C:\\", 1024));

            // Maximum 512 mb system ram area (for virtual).
            builder.AddVirtualMemorySizeHealthCheck(512);

            // Maximum 512 mb system ram area (for private).
            builder.AddPrivateMemoryHealthCheck(512);

            services.AddDbConfiguration(Configuration);
        }

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
