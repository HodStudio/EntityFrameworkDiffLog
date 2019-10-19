using HodStudio.EfDiffLog.Model;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
#endif

namespace HodStudio.EfDiffLog.Repository
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext(params Assembly[] assemblies)
        {
            InitializeAssemblies(assemblies);
        }

#if NETSTANDARD
        public LoggingDbContext(DbContextOptions<LoggingDbContext> options, params Assembly[] assemblies) : base(options)
        {
            InitializeAssemblies(assemblies);
        }
#else
        public LoggingDbContext(string nameOrConnectionString, params Assembly[] assemblies) : base(nameOrConnectionString)
        {
            InitializeAssemblies(assemblies);
        }

        public LoggingDbContext(string nameOrConnectionString, DbCompiledModel model, params Assembly[] assemblies) : base(nameOrConnectionString, model)
        {
            InitializeAssemblies(assemblies);
        }

        public LoggingDbContext(DbConnection existingConnection, bool contextOwnsConnection, params Assembly[] assemblies) : base(existingConnection, contextOwnsConnection)
        {
            InitializeAssemblies(assemblies);
        }

        public LoggingDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext, params Assembly[] assemblies) : base(objectContext, dbContextOwnsObjectContext)
        {
            InitializeAssemblies(assemblies);
        }

        public LoggingDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection, params Assembly[] assemblies) : base(existingConnection, model, contextOwnsConnection)
        {
            InitializeAssemblies(assemblies);
        }

        protected LoggingDbContext(DbCompiledModel model, params Assembly[] assemblies) : base(model)
        {
            InitializeAssemblies(assemblies);
        }
#endif

        public DbSet<LogEntry> LogEntries { get; set; }

        public string UserId { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.LogChanges(UserId);
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void InitializeAssemblies(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
                this.InitializeIdColumnNames(assembly);
        }
    }
}
