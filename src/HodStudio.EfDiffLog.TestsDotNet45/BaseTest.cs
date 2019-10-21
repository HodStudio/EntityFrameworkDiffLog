using HodStudio.EfDiffLog.Repository;
using HodStudio.EfDiffLog.TestsDotNetCore.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HodStudio.EfDiffLog.TestsDotNetCore
{
    public class BaseTest
    {
        public static bool Initialized = false;

        protected AppDbContext Context { get; private set; }

        protected static DbContextOptions<AppDbContext> Options { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            if (!Initialized)
            {
                var builder = new DbContextOptionsBuilder<AppDbContext>();
                builder.UseInMemoryDatabase($"HsEfDiffLog");
                Options = builder.Options;

                LoggingContext.InitializeIdColumnNames(typeof(AppDbContext).Assembly);
                Initialized = true;
            }
        }

        [SetUp]
        public void SetUp()
        {
            CreateContext();

            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        protected void CreateContext()
            => Context = new AppDbContext(Options);
    }
}
