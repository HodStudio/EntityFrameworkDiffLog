using HodStudio.EfDiffLog.Repository;
using HodStudio.EfDiffLog.TestsDotNet45.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace HodStudio.EfDiffLog.TestsDotNetCore
{
    public class BaseTest
    {
        protected AppDbContext Context { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            LoggingContext.InitializeIdColumnNames(typeof(AppDbContext).Assembly);
        }

        [SetUp]
        public void SetUp()
        {
            DbContextOptions<AppDbContext> options;
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase($"HsEfDiffLog-{Guid.NewGuid()}");
            options = builder.Options;

            Context = new AppDbContext(options);

            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }
    }
}
