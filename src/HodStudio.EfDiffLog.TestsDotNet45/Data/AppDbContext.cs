using HodStudio.EfDiffLog.Repository;
using HodStudio.EfDiffLog.TestsDotNet45.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HodStudio.EfDiffLog.TestsDotNet45.Data
{
    public class AppDbContext : LoggingDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
