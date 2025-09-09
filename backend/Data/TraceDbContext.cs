using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class TraceDbContext : DbContext
    {
        public TraceDbContext(DbContextOptions<TraceDbContext> options) : base(options)
        {
        }

        public DbSet<TraceEntity> Traces => Set<TraceEntity>();
    }
}
