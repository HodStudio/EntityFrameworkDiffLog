using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EfDiffLog.TestsDotNetCore.IdGeneratedByApp
{
    public class InsertOperationsTests : IdGeneratedByAppBaseTests
    {
        protected override string Operation { get; set; } = "Added";

        [Test]
        public void SaveChanges()
        {
            var department = PrepareDepartment();
            Context.SaveChanges();
            Validate(department);
        }

        [Test]
        public void SaveChangesWithAcceptAllChangesOnSuccessTrue()
        {
            var department = PrepareDepartment();
            Context.SaveChanges(true);
            ValidateNoLog(department);
        }

        [Test]
        public void SaveChangesWithAcceptAllChangesOnSuccessFalse()
        {
            var department = PrepareDepartment();
            Context.SaveChanges(true);
            ValidateNoLog(department);
        }

        [Test]
        public async Task SaveChangesAsync()
        {
            var department = PrepareDepartment();
            await Context.SaveChangesAsync();
            Validate(department);
        }

        [Test]
        public async Task SaveChangesAsyncCancelationToken()
        {
            var department = PrepareDepartment();
            var cancelToken = new CancellationToken();
            await Context.SaveChangesAsync(cancelToken);
            Validate(department);
        }

        [Test]
        public async Task SaveChangesWithAcceptAllChangesOnSuccessTrueAsync()
        {
            var department = PrepareDepartment();
            await Context.SaveChangesAsync(true);
            ValidateNoLog(department);
        }

        [Test]
        public async Task SaveChangesWithAcceptAllChangesOnSuccessFalseAsync()
        {
            var department = PrepareDepartment();
            await Context.SaveChangesAsync(false);
            ValidateNoLog(department);
        }
    }
}
