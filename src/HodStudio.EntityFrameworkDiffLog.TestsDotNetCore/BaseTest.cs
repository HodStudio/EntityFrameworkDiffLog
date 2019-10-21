using HodStudio.EntityFrameworkDiffLog.Repository;
using HodStudio.EntityFrameworkDiffLog.TestsDotNetCore.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNetCore
{
    public class BaseTest
    {
        public static bool Initialized = false;

        protected virtual string Operation { get; set; }

        protected AppDbContext Context { get; private set; }

        protected static DbContextOptions<AppDbContext> Options { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            if (!Initialized)
            {
                var builder = new DbContextOptionsBuilder<AppDbContext>();
                builder.UseInMemoryDatabase($"HsEntityFrameworkDiffLog");
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
