using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EfDiffLog.TestsDotNetCore.IdGeneratedByDatabase
{
    public class InsertOperationsTests : IdGeneratedByDatabaseBaseTests
    {
        protected override string Operation { get; set; } = "Added";

        [Test]
        public void SaveChanges()
        {
            var user = PrepareUser();
            Context.SaveChanges();
            Validate(user);
        }

        [Test]
        public void SaveChangesWithAcceptAllChangesOnSuccessTrue()
        {
            var user = PrepareUser();
            Context.SaveChanges(true);
            ValidateNoLog(user);
        }

        [Test]
        public void SaveChangesWithAcceptAllChangesOnSuccessFalse()
        {
            var user = PrepareUser();
            Context.SaveChanges(true);
            ValidateNoLog(user);
        }

        [Test]
        public async Task SaveChangesAsync()
        {
            var user = PrepareUser();
            await Context.SaveChangesAsync();
            Validate(user);
        }

        [Test]
        public async Task SaveChangesAsyncCancelationToken()
        {
            var user = PrepareUser();
            var cancelToken = new CancellationToken();
            await Context.SaveChangesAsync(cancelToken);
            Validate(user);
        }

        [Test]
        public async Task SaveChangesWithAcceptAllChangesOnSuccessTrueAsync()
        {
            var user = PrepareUser();
            await Context.SaveChangesAsync(true);
            ValidateNoLog(user);
        }

        [Test]
        public async Task SaveChangesWithAcceptAllChangesOnSuccessFalseAsync()
        {
            var user = PrepareUser();
            await Context.SaveChangesAsync(false);
            ValidateNoLog(user);
        }
    }
}
