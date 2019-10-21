using HodStudio.EntityFrameworkDiffLog.Model;
using System.Threading;
using System.Threading.Tasks;
using System;

#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HodStudio.EntityFrameworkDiffLog.Repository
{
    public partial class LoggingDbContext
    {
        protected string LogEntriesTableName { get; set; } = "LogEntries";
        protected string LogEntriesSchemaName { get; set; } = "dbo";

        public bool IdGeneratedByDatabase { get; set; } = true;

        public DbSet<LogEntry> LogEntries { get; set; }

        public string UserId { get; set; }

        public override int SaveChanges()
        {
            this.LogChanges(UserId);
            var result = base.SaveChanges();
            if (IdGeneratedByDatabase)
            {
                this.LogChangesAddedEntities(UserId);
                result = base.SaveChanges();
            }
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await this.LogChangesAsync(UserId);
            var result = await base.SaveChangesAsync(cancellationToken);
            if (IdGeneratedByDatabase)
            {
                await this.LogChangesAddedEntitiesAsync(UserId);
                result = await base.SaveChangesAsync(cancellationToken);
            }
            return result;
        }
    }
}
