#if NETSTANDARD
using HodStudio.EntityFrameworkDiffLog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EntityFrameworkDiffLog.Repository
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

        /// <summary>
        /// Saves all changes made in this context to the database and create diff logs using the HodStudio.EntityFrameworkDiffLog.
        /// 
        /// Please, pay attention that direct calls to this methods doesn't produce the EntityFrameworkDiffLogs.
        /// </summary>
        /// <remarks> 
        /// This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
        /// to discover any changes to entity instances before saving to the underlying database.
        /// This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
        /// 
        /// Please, pay attention that direct calls to this methods doesn't produce the EntityFrameworkDiffLogs.
        /// Use the overload that has no parameters \"SaveChangesAsync()\" or with the CancellationToken.
        /// </remarks>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges is called after the changes have been sent successfully to the database.</param>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// Saves all changes made in this context to the database and create diff logs using the HodStudio.EntityFrameworkDiffLog.
        /// 
        /// Please, pay attention that direct calls to this methods doesn't produce the EntityFrameworkDiffLogs.
        /// </summary>
        /// <remarks> 
        /// This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
        /// to discover any changes to entity instances before saving to the underlying database.
        /// This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
        /// Multiple active operations on the same context instance are not supported. Use
        /// 'await' to ensure that any asynchronous operations have completed before calling
        /// another method on this context.
        /// 
        /// Please, pay attention that direct calls to this methods doesn't produce the EntityFrameworkDiffLogs.
        /// Use the overload that has no parameters \"SaveChangesAsync()\" or with the CancellationToken.
        /// </remarks>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges is called after the changes have been sent successfully to the database.</param>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
#endif