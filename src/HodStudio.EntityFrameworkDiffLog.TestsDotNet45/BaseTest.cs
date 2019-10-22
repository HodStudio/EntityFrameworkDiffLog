using HodStudio.EntityFrameworkDiffLog.Repository;
using HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Data;
using NUnit.Framework;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45
{
    public class BaseTest
    {
        public static bool Initialized = false;

        protected virtual string Operation { get; set; }

        protected AppDbContext Context { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            if (!Initialized)
            {
                LoggingContext.InitializeIdColumnNames(typeof(AppDbContext).Assembly);
                Initialized = true;
            }
        }

        [SetUp]
        public void SetUp()
        {
            CreateContext();

            Context.Database.Delete();
            Context.Database.Create();
        }

        protected void CreateContext()
            => Context = new AppDbContext();
    }
}
