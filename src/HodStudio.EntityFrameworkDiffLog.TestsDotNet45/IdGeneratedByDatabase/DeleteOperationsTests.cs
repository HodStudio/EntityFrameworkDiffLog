using HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Model;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45.IdGeneratedByDatabase
{
    public class DeleteOperationsTests : IdGeneratedByDatabaseBaseTests
    {
        protected override string Operation { get; set; } = "Deleted";

        protected override User PrepareUser()
        {
            var original = base.PrepareUser();
            Context.SaveChanges();

            CreateContext();
            var toDelete = Context.Users.Find(original.Id);

            Context.Users.Remove(toDelete);
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
    }
}
