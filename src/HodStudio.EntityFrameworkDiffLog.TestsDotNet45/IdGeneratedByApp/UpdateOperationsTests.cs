using HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45.IdGeneratedByApp
{
    public class UpdateOperationsTests : IdGeneratedByAppBaseTests
    {
        protected override string Operation { get; set; } = "Modified";

        protected new(Department, Department) PrepareDepartment()
        {
            var original = base.PrepareDepartment();
            Context.SaveChanges();

            CreateContext();

            Thread.Sleep(100);

            var updated = Context.Departments.Find(original.Id);
            var newDepartment = CreateDepartment();
            updated.Budget = original.Budget + 10;
            updated.Code = newDepartment.Code;

            return (original, updated);
        }

        protected void Validate(Department original, Department updated)
        {
            ValidateDepartment(updated);

            var currentLog = GetLog(updated);
            Assert.IsNotNull(currentLog);

            var expected = string.Format("{{\"Code\":[\"{2}\",\"{3}\"],\"Budget\":[{0},{1}]}}",
                original.Budget, updated.Budget,
                original.Code, updated.Code);
            Assert.AreEqual(expected, currentLog.ValuesChanges);
        }

        [Test]
        public void SaveChanges()
        {
            var (original, updated) = PrepareDepartment();
            Context.SaveChanges();
            Validate(original, updated);
        }

        [Test]
        public async Task SaveChangesAsync()
        {
            var (original, updated) = PrepareDepartment();
            await Context.SaveChangesAsync();
            Validate(original, updated);
        }

        [Test]
        public async Task SaveChangesAsyncCancelationToken()
        {
            var (original, updated) = PrepareDepartment();
            var cancelToken = new CancellationToken();
            await Context.SaveChangesAsync(cancelToken);
            Validate(original, updated);
        }
    }
}
