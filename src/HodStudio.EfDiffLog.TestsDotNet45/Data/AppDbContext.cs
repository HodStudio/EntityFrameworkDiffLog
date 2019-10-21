using HodStudio.EfDiffLog.Repository;
using HodStudio.EfDiffLog.TestsDotNet45.Model;
using Microsoft.EntityFrameworkCore;

namespace HodStudio.EfDiffLog.TestsDotNet45.Data
{
    public class AppDbContext : LoggingDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
