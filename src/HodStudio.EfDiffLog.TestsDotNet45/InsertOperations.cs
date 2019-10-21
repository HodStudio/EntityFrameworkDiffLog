using HodStudio.EfDiffLog.TestsDotNet45.Model;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HodStudio.EfDiffLog.TestsDotNetCore
{
    public class InsertOperations : BaseTest
    {
        private User PrepareUser()
        {
            var uniqueId = Guid.NewGuid().ToString();
            var user = new User() { Name = uniqueId, Email = $"{uniqueId}@hodstudio.com.br" };
            Context.Users.Add(user);
            return user;
        }

        private void Validate(User user)
        {
            var logs = from l in Context.LogEntries select l;
            var currentLog = logs.FirstOrDefault(x =>
                x.EntityName == typeof(User).Name
                && x.Operation == "Added"
                && x.EntityId == user.Id.ToString());

            Assert.IsNotNull(currentLog);

            var expected = string.Format("{{\"Id\":[{0}],\"Email\":[\"{1}@hodstudio.com.br\"],\"Name\":[\"{1}\"]}}", 
                user.Id, user.Name);
            Assert.AreEqual(expected, currentLog.ValuesChanges);
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
            Validate(user);
        }

        [Test]
        public void SaveChangesWithAcceptAllChangesOnSuccessFalse()
        {
            var user = PrepareUser();
            Context.SaveChanges(true);
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
        public async Task SaveChangesWithAcceptAllChangesOnSuccessTrueAsync()
        {
            var user = PrepareUser();
            await Context.SaveChangesAsync(true);
            Validate(user);
        }

        [Test]
        public async Task SaveChangesWithAcceptAllChangesOnSuccessFalseAsync()
        {
            var user = PrepareUser();
            await Context.SaveChangesAsync(false);
            Validate(user);
        }
    }
}
