using HodStudio.EfDiffLog.TestsDotNet45.Model;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HodStudio.EfDiffLog.TestsDotNetCore
{
    public class CrudOperationsTests : BaseTest
    {
        [Test]
        public void TestInsertSaveChanges()
        {
            var user = new User() { Name = "Test Insert", Email = "insert@hodstudio.com.br" };
            Context.Users.Add(user);
            Context.SaveChanges();

            var logs = from l in Context.LogEntries select l;
            var currentLog = logs.FirstOrDefault(x => 
                x.EntityName == typeof(User).Name
                && x.Operation == "Added"
                && x.EntityId == user.Id.ToString());

            Assert.IsNotNull(currentLog);

            var expected = string.Format("{{\"Id\":[{0}],\"Email\":[\"insert@hodstudio.com.br\"],\"Name\":[\"Test Insert\"]}}", user.Id);
            Assert.AreEqual(expected, currentLog.ValuesChanges);   
        }

        [Test]
        public void TestUpdateSaveChanges()
        {
            var user = new User() { Name = "Test Update before", Email = "update.before@hodstudio.com.br" };
            Context.Users.Add(user);
            Context.SaveChanges();

            user.Name = "Test Update after";
            user.Email = "update.after@hodstudio.com.br";

            Context.Update(user);
            Context.SaveChanges();

            var logs = from l in Context.LogEntries select l;
            var currentLog = logs.FirstOrDefault(x =>
                x.EntityName == typeof(User).Name
                && x.Operation == "Modified"
                && x.EntityId == user.Id.ToString());

            Assert.IsNotNull(currentLog);
            Assert.AreEqual("{\"Email\":[\"update.before@hodstudio.com.br\",\"update.after@hodstudio.com.br\"],\"Name\":[\"Test Update before\",\"Test Update after\"]}", currentLog.ValuesChanges);
        }

        [Test]
        public void TestDeleteSaveChanges()
        {
            var user = new User() { Name = "Test Delete", Email = "delete@hodstudio.com.br" };
            Context.Users.Add(user);
            Context.SaveChanges();

            Context.Remove(user);
            Context.SaveChanges();

            var logs = from l in Context.LogEntries select l;
            var currentLog = logs.FirstOrDefault(x =>
                x.EntityName == typeof(User).Name
                && x.Operation == "Deleted"
                && x.EntityId == user.Id.ToString());

            Assert.IsNotNull(currentLog);

            var expected = string.Format("{{\"Id\":[{0}],\"Email\":[\"delete@hodstudio.com.br\"],\"Name\":[\"Test Delete\"]}}", user.Id);
            Assert.AreEqual(expected, currentLog.ValuesChanges);
        }

        [Test]
        public async Task TestInsertSaveChangesAsync()
        {

        }
    }
}
