using HodStudio.EfDiffLog.Model;
using System.Threading;
using System.Threading.Tasks;

#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HodStudio.EfDiffLog.Repository
{
    public class LoggingDbContext : DbContext
    {
        protected string LogEntriesTableName { get; set; } = "LogEntries";
        protected string LogEntriesSchemaName { get; set; } = "dbo";
        public DbSet<LogEntry> LogEntries { get; set; }

        public string UserId { get; set; }

#if NETSTANDARD
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().ToTable(LogEntriesTableName);
            base.OnModelCreating(modelBuilder);
        }
#else
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().ToTable(LogEntriesTableName, LogEntriesSchemaName);
            base.OnModelCreating(modelBuilder);
        }
#endif

        public override int SaveChanges()
        {
            this.LogChanges(UserId);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.LogChangesAsync(UserId);
            return await base.SaveChangesAsync(cancellationToken);
        }

#if NETFULL
        public override Task<int> SaveChangesAsync()
        {
            this.LogChanges(UserId);
            return base.SaveChangesAsync();
        }
#endif

    }
}
