using HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Model;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45.IdGeneratedByApp
{
    public class DeleteOperationsTests : IdGeneratedByAppBaseTests
    {
        protected override string Operation { get; set; } = "Deleted";

        protected override Department PrepareDepartment()
        {
            var original = base.PrepareDepartment();
            Context.SaveChanges();

            CreateContext();
            var toDelete = Context.Departments.Find(original.Id);

            Context.Departments.Remove(toDelete);
            return original;
        }

        protected override void ValidateDepartment(Department department)
        {
            Assert.IsNull(GetDepartment(department));
        }

        [Test]
        public void SaveChanges()
        {
            var department = PrepareDepartment();
            Context.SaveChanges();
            Validate(department);
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
    }
}
