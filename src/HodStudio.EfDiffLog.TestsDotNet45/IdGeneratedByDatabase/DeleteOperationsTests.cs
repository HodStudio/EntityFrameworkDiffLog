using HodStudio.EfDiffLog.TestsDotNetCore.Model;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EfDiffLog.TestsDotNetCore.IdGeneratedByDatabase
{
    public class DeleteOperationsTests : IdGeneratedByDatabaseBaseTests
    {
        protected override string Operation { get; set; } = "Deleted";

        protected override User PrepareUser()
        {
            var original = base.PrepareUser();
            Context.SaveChanges();

            CreateContext();

            Context.Users.Remove(original);
            return original;
        }

        protected override void ValidateUser(User user)
        {
            Assert.IsNull(GetUser(user));
        }

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
            CreateContext();
            ValidateNoLog(user);
        }
    }
}
