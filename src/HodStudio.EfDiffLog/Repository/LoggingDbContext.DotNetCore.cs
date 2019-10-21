#if NETSTANDARD
using HodStudio.EfDiffLog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.LogChanges(UserId);
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            if (IdGeneratedByDatabase)
            {
                this.LogChangesAddedEntities(UserId);
                result = base.SaveChanges(acceptAllChangesOnSuccess);
            }
            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            await this.LogChangesAsync(UserId);
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            if (IdGeneratedByDatabase)
            {
                await this.LogChangesAddedEntitiesAsync(UserId);
                result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            return result;
        }
    }
}
#endif