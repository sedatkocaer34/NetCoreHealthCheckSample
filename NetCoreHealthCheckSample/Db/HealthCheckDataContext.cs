using Microsoft.EntityFrameworkCore;
using NetCoreHealthCheckSample.Models;

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
