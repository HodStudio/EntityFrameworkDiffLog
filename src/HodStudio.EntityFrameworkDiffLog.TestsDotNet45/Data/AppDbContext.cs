using HodStudio.EntityFrameworkDiffLog.Repository;
using HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Model;
using System.Data.Entity;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Data
{
    public class AppDbContext : LoggingDbContext
    {
        public AppDbContext() : base("SchoolContext") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
