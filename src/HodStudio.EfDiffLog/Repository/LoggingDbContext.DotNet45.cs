#if NETFULL
using HodStudio.EfDiffLog.Model;
using System;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace HodStudio.EfDiffLog.Repository
{
    public partial class LoggingDbContext : DbContext
    {
        internal List<DbEntityEntry> AddedEntities { get; set; } = new List<DbEntityEntry>();
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().ToTable(LogEntriesTableName, LogEntriesSchemaName);
            base.OnModelCreating(modelBuilder);
        }

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

        public override Task<int> SaveChangesAsync()
        {
            if (IdGeneratedByDatabase)
                throw new InvalidOperationException("You can't use the SaveChangesAsync without the await parameter if you have the \"IdGeneratedByDatabase\" configured to true. Use the overload that uses the await call.");

            this.LogChanges(UserId);
            return base.SaveChangesAsync();
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
#endif