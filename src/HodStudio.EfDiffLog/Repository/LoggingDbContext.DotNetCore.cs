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

        /// <summary>
        /// Saves all changes made in this context to the database and create diff logs using the HodStudio.EfDiffLog.
        /// 
        /// Please, pay attention that if you are using the EfDiffLog.IdGeneratedByDatabase configured to true, this method should not be used.
        /// </summary>
        /// <remarks> 
        /// This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
        /// to discover any changes to entity instances before saving to the underlying database.
        /// This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
        /// 
        /// Please, pay attention that if you are using the EfDiffLog.IdGeneratedByDatabase configured to true, this method should not be used.
        /// To continue using this overload, configure the \"IdGeneratedByDatabase\" to false, or use the overload that has no parameters \"SaveChanges()\".
        /// </remarks>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges is called after the changes have been sent successfully to the database.</param>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        /// <exception cref="InvalidOperationException">You can't use the SaveChangesAsync with the \"acceptAllChangesOnSuccess\" parameter while using the \"IdGeneratedByDatabase\" configured to true. Please, to continue using this overload, configure the \"IdGeneratedByDatabase\" to false, or use the overload that has no parameters \"SaveChanges()\".</exception>
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