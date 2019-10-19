using HodStudio.EfDiffLog.Model;
using System.Reflection;
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
        public DbSet<LogEntry> LogEntries { get; set; }

        public string UserId { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.LogChanges(UserId);
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
