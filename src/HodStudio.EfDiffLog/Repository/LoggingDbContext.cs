using HodStudio.EfDiffLog.Model;
using System.Threading;
using System.Threading.Tasks;

#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
#endif

namespace HodStudio.EfDiffLog.Repository
{
    public class LoggingDbContext : DbContext
    {

#if NETSTANDARD
        public LoggingDbContext(DbContextOptions options) : base(options) { }

        protected LoggingDbContext() { }
#else
        protected LoggingDbContext()
        {
        }

        protected LoggingDbContext(DbCompiledModel model) : base(model)
        {
        }

        public LoggingDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public LoggingDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }

        public LoggingDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public LoggingDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
        }

        public LoggingDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext)
        {
        }
#endif

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
