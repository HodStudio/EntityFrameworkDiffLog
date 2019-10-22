using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45.IdGeneratedByDatabase
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
        public async Task SaveChangesAsync()
        {
            var user = PrepareUser();
            await Context.SaveChangesAsync();
            ValidateUser(user);
            //Assert.ThrowsAsync<InvalidOperationException>(async () => await Context.SaveChangesAsync());
        }

        [Test]
        public async Task SaveChangesAsyncCancelationToken()
        {
            var user = PrepareUser();
            var cancelToken = new CancellationToken();
            await Context.SaveChangesAsync(cancelToken);
            Validate(user);
        }
    }
}
