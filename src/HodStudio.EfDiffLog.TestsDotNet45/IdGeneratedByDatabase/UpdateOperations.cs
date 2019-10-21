using HodStudio.EfDiffLog.TestsDotNet45.Model;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EfDiffLog.TestsDotNetCore.IdGeneratedByDatabase
{
    public class UpdateOperations : IdGeneratedByDatabaseBaseTest
    {
        protected override string Operation { get; set; } = "Modified";

        protected new(User, User) PrepareUser()
        {
            var original = base.PrepareUser();
            Context.SaveChanges();

            CreateContext();

            var updated = CreateUser();
            updated.Id = original.Id;

            Context.Users.Update(updated);
            return (original, updated);
        }

        protected void Validate(User original, User updated)
        {
            ValidateUser(updated);

            var currentLog = GetLog(updated);
            Assert.IsNotNull(currentLog);

            var expected = string.Format("{{\"Email\":[\"{0}\",\"{1}\"],\"Name\":[\"{2}\",\"{3}\"]}}",
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
        public void SaveChangesWithAcceptAllChangesOnSuccessTrue()
        {
            var (_, updated) = PrepareUser();
            Context.SaveChanges(true);
            ValidateNoLog(updated);
        }

        [Test]
        public void SaveChangesWithAcceptAllChangesOnSuccessFalse()
        {
            var (_, updated) = PrepareUser();
            Context.SaveChanges(true);
            ValidateNoLog(updated);
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

        [Test]
        public async Task SaveChangesWithAcceptAllChangesOnSuccessTrueAsync()
        {
            var (_, updated) = PrepareUser();
            await Context.SaveChangesAsync(true);
            ValidateNoLog(updated);
        }

        [Test]
        public async Task SaveChangesWithAcceptAllChangesOnSuccessFalseAsync()
        {
            var (_, updated) = PrepareUser();
            await Context.SaveChangesAsync(false);
            ValidateNoLog(updated);
        }
    }
}
