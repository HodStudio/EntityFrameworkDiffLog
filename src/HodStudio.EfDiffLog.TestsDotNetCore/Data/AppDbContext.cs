using HodStudio.EfDiffLog.Repository;
using HodStudio.EfDiffLog.TestsDotNetCore.Model;
using Microsoft.EntityFrameworkCore;

namespace HodStudio.EfDiffLog.TestsDotNetCore.Data
{
    public class AppDbContext : LoggingDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
