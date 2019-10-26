using HodStudio.EntityFrameworkDiffLog.TestsDotNetCore.Model;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNetCore.IdGeneratedByApp
{
    public class UpdateOperationsTests : IdGeneratedByAppBaseTests
    {
        protected override string Operation { get; set; } = "Modified";

        protected new(Department, Department) PrepareDepartment()
        {
            var original = base.PrepareDepartment();
            Context.SaveChanges();

            CreateContext();

            var updated = CreateDepartment();
            updated.Id = original.Id;
            updated.Budget = original.Budget + 10;

            Context.Departments.Update(updated);
            return (original, updated);
        }

        protected void Validate(Department original, Department updated)
        {
            ValidateDepartment(updated);

            var currentLog = GetLog(updated);
            Assert.IsNotNull(currentLog);

            var expected = string.Format("{{\"Budget\":[{0:N1},{1:N1}],\"EffectiveFrom\":[\"{2:yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz}\",\"{3:yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz}\"]}}",
                original.Budget, updated.Budget,
                original.EffectiveFrom, updated.EffectiveFrom);
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
        public void SaveChangesWithAcceptAllChangesOnSuccessTrue()
        {
            var (_, updated) = PrepareDepartment();
            Context.SaveChanges(true);
            ValidateNoLog(updated);
        }

        [Test]
        public void SaveChangesWithAcceptAllChangesOnSuccessFalse()
        {
            var (_, updated) = PrepareDepartment();
            Context.SaveChanges(true);
            ValidateNoLog(updated);
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

        [Test]
        public async Task SaveChangesWithAcceptAllChangesOnSuccessTrueAsync()
        {
            var (_, updated) = PrepareDepartment();
            await Context.SaveChangesAsync(true);
            ValidateNoLog(updated);
        }

        [Test]
        public async Task SaveChangesWithAcceptAllChangesOnSuccessFalseAsync()
        {
            var (_, updated) = PrepareDepartment();
            await Context.SaveChangesAsync(false);
            ValidateNoLog(updated);
        }
    }
}
