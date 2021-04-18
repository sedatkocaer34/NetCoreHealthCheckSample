using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NetCoreHealthCheckSample.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreHealthCheckSample.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void  AddDbConfiguration(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddDbContext<HealthCheckDataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
