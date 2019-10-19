#if NETSTANDARD
using HodStudio.EfDiffLog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace HodStudio.EfDiffLog.Repository
{
    public partial class LoggingDbContext : DbContext
    {
        public LoggingDbContext(DbContextOptions options) : base(options) { }

        protected LoggingDbContext() { }

        internal List<EntityEntry> AddedEntities { get; set; } = new List<EntityEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().ToTable(LogEntriesTableName, LogEntriesSchemaName);
            base.OnModelCreating(modelBuilder);
        }
    }
}
#endif