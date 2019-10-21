using HodStudio.EfDiffLog.Model;
using HodStudio.EfDiffLog.TestsDotNet45.Model;
using NUnit.Framework;
using System;
using System.Linq;

namespace HodStudio.EfDiffLog.TestsDotNetCore.IdGeneratedByDatabase
{
    public class IdGeneratedByDatabaseBaseTest : BaseTest
    {
        protected virtual string Operation { get; set; }

        [SetUp]
        public void SetUp() => Context.IdGeneratedByDatabase = true;

        protected virtual User CreateUser()
        {
            var uniqueId = Guid.NewGuid().ToString("N");
            return new User() { Name = uniqueId, Email = $"{uniqueId}@hodstudio.com.br" };
        }

        protected virtual User PrepareUser()
        {
            var user = CreateUser();
            Context.Users.Add(user);
            return user;
        }

        protected LogEntry GetLog(User user)
            => Context.LogEntries.FirstOrDefault(x =>
                x.EntityName == typeof(User).Name
                && x.Operation == Operation
                && x.EntityId == user.Id.ToString());

        protected User GetUser(User user)
            => Context.Users.Find(user.Id);

        protected virtual void Validate(User user)
        {
            ValidateUser(user);
            ValidateLog(user);
        }

        protected virtual void ValidateUser(User user)
            => Assert.IsNotNull(GetUser(user));

        protected virtual void ValidateLog(User user)
        {
            var currentLog = GetLog(user);
            Assert.IsNotNull(currentLog);

            var expected = string.Format("{{\"Id\":[{0}],\"Email\":[\"{1}@hodstudio.com.br\"],\"Name\":[\"{1}\"]}}",
                user.Id, user.Name);
            Assert.AreEqual(expected, currentLog.ValuesChanges);
        }

        protected void ValidateNoLog(User user)
        {
            ValidateUser(user);
            Assert.IsNull(GetLog(user));
        }
    }
}
