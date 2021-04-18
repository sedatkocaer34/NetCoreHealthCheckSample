using Microsoft.EntityFrameworkCore;
using NetCoreHealthCheckSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreHealthCheckSample.Db
{
    public class HealthCheckDataContext:DbContext
    {
        public HealthCheckDataContext(DbContextOptions<HealthCheckDataContext> options):base(options)
        {

        }

        public DbSet<Todo> Todo { get; set; }
    }
}
