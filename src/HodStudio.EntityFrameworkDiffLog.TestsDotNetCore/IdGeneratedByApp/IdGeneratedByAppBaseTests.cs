using HodStudio.EntityFrameworkDiffLog.Model;
using HodStudio.EntityFrameworkDiffLog.TestsDotNetCore.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNetCore.IdGeneratedByApp
{
    public class IdGeneratedByAppBaseTests : BaseTest
    {
        [SetUp]
        public void SetUpAppTests() => Context.IdGeneratedByDatabase = false;

        protected virtual Department CreateDepartment()
            => new Department()
            {
                EffectiveFrom = DateTime.Now.AddDays(-DateTime.Now.Millisecond),
                Budget = DateTime.Now.Millisecond
            };

        protected virtual Department PrepareDepartment()
        {
            var department = CreateDepartment();
            Context.Departments.Add(department);
            return department;
        }

        protected LogEntry GetLog(Department department)
            => Context.LogEntries.FirstOrDefault(x =>
                x.EntityName == typeof(Department).Name
                && x.Operation == Operation
                && x.EntityId == department.Id.ToString());

        protected Department GetDepartment(Department department)
            => Context.Departments.Find(department.Id);

        protected virtual void Validate(Department department)
        {
            ValidateDepartment(department);
            ValidateLog(department);
        }

        protected virtual void ValidateDepartment(Department department)
            => Assert.IsNotNull(GetDepartment(department));

        protected virtual void ValidateLog(Department department)
        {
            var currentLog = GetLog(department);
            Assert.IsNotNull(currentLog);

            var expected = string.Format("{{\"Id\":[\"{0}\"],\"Budget\":[{1:F1}],\"EffectiveFrom\":[\"{2:yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz}\"]}}",
                department.Id, department.Budget, department.EffectiveFrom);
            Assert.AreEqual(expected, currentLog.ValuesChanges);
        }

        protected void ValidateNoLog(Department department)
        {
            ValidateDepartment(department);
            Assert.IsNull(GetLog(department));
        }
    }
}
