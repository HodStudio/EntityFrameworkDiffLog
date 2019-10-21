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

        public bool IdGeneratedByDatabase { get; set; } = true;

        public DbSet<LogEntry> LogEntries { get; set; }

        public string UserId { get; set; }

        

        

        
    }
}
