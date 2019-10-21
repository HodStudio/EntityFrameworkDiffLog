using HodStudio.EntityFrameworkDiffLog.Repository;
using HodStudio.EntityFrameworkDiffLog.TestsDotNetCore.Model;
using Microsoft.EntityFrameworkCore;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNetCore.Data
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
