using HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Model;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45.IdGeneratedByDatabase
{
    public class UpdateOperationsTests : IdGeneratedByDatabaseBaseTests
    {
        protected override string Operation { get; set; } = "Modified";

        protected new(User, User) PrepareUser()
        {
            var original = base.PrepareUser();
            Context.SaveChanges();

            CreateContext();

            var updated = Context.Users.Find(original.Id);
            var newUser = CreateUser();
            updated.Id = original.Id;
            updated.Name = newUser.Name;
            updated.Email = newUser.Email;

            return (original, updated);
        }

        protected void Validate(User original, User updated)
        {
            ValidateUser(updated);

            var currentLog = GetLog(updated);
            Assert.IsNotNull(currentLog);

            var expected = string.Format("{{\"Name\":[\"{2}\",\"{3}\"],\"Email\":[\"{0}\",\"{1}\"]}}",
                original.Email, updated.Email,
                original.Name, updated.Name);
            Assert.AreEqual(expected, currentLog.ValuesChanges);
        }

        [Test]
        public void SaveChanges()
        {
            var (original, updated) = PrepareUser();
            Context.SaveChanges();
            Validate(original, updated);
        }

        [Test]
        public async Task SaveChangesAsync()
        {
            var (original, updated) = PrepareUser();
            await Context.SaveChangesAsync();
            Validate(original, updated);
        }

        [Test]
        public async Task SaveChangesAsyncCancelationToken()
        {
            var (original, updated) = PrepareUser();
            var cancelToken = new CancellationToken();
            await Context.SaveChangesAsync(cancelToken);
            Validate(original, updated);
        }
    }
}
