#if NETFULL
using HodStudio.EntityFrameworkDiffLog.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace HodStudio.EntityFrameworkDiffLog.Repository
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

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous save operation. 
        /// Because we create the logs in run time, the return will be the number of logs
        /// related saved on the database, and not the original from the method.
        /// In case that you uses "IdGeneratedByDatabase", the first save will not be 
        /// asyncronous, only the second one. We recommend to use the version with 
        /// the CancellationToken, which uses the await.
        /// </returns>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported. Use
        /// 'await' to ensure that any asynchronous operations have completed before calling
        /// another method on this context.
        /// </remarks>
        public override Task<int> SaveChangesAsync()
        {
            this.LogChanges(UserId);
            if (!IdGeneratedByDatabase)
                return base.SaveChangesAsync();

            base.SaveChangesAsync().GetAwaiter().GetResult();
            this.LogChangesAddedEntities(UserId);
            return base.SaveChangesAsync();
        }
    }
}
#endif