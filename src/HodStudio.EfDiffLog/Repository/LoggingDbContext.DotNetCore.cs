#if NETSTANDARD
using HodStudio.EfDiffLog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
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
            if (IdGeneratedByDatabase)
                throw new InvalidOperationException("You can't use the SaveChangesAsync with the \"acceptAllChangesOnSuccess\" parameter while using the \"IdGeneratedByDatabase\" configured to true. Please, to continue using this overload, configure the \"IdGeneratedByDatabase\" to false, or use the overload that has no parameters \"SaveChanges()\".");

            this.LogChanges(UserId);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (IdGeneratedByDatabase)
                throw new InvalidOperationException("You can't use the SaveChangesAsync with the \"acceptAllChangesOnSuccess\" parameter while using the \"IdGeneratedByDatabase\" configured to true. Please, to continue using this overload, configure the \"IdGeneratedByDatabase\" to false, or use the overload that has no parameters \"SaveChangesAsync()\".");

            await this.LogChangesAsync(UserId);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
#endif