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
    public partial class LoggingDbContext
    {
        protected string LogEntriesTableName { get; set; } = "LogEntries";
        protected string LogEntriesSchemaName { get; set; } = "dbo";
        public DbSet<LogEntry> LogEntries { get; set; }

        public string UserId { get; set; }

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
    }
}
