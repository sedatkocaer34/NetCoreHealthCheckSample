using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreHealthCheckSample.Configuration
{
    public static class HealthCheckConfiguration
    {
        public static void AddHealthCheckConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecksUI().AddInMemoryStorage();

            var builder = services.AddHealthChecks();
            builder.AddSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                healthQuery: "select 1",
                name: "Sql Server",
                failureStatus: null,
                tags: null,
                timeout: TimeSpan.FromSeconds(5));

            builder.AddRedis(redisConnectionString: configuration.GetConnectionString("Redis"),
            failureStatus: HealthStatus.Degraded);

            // Minimum 1 Gb empty system disk area.
            builder.AddDiskStorageHealthCheck(options => options.AddDrive("C:\\", 1024));

            // Maximum 512 mb system ram area (for virtual).
            builder.AddVirtualMemorySizeHealthCheck(512);

            // Maximum 512 mb system ram area (for private).
            builder.AddPrivateMemoryHealthCheck(512);
        }
    }
}
