using HodStudio.EntityFrameworkDiffLog.Model;
using System;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Model
{
    [LoggedEntity]
    public class Department
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid Code { get; set; }
        public long Budget { get; set; }
    }
}
