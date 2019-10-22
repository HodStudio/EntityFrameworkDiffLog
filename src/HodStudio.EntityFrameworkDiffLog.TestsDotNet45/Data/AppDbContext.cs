using HodStudio.EntityFrameworkDiffLog.Repository;
using HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Model;
using System.Data.Entity;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Data
{
    public class AppDbContext : LoggingDbContext
    {
        public AppDbContext() : base("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=HsEntityFrameworkDiffLog;Integrated Security=SSPI;") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
