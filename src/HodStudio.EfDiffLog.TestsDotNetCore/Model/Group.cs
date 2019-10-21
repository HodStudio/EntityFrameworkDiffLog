using HodStudio.EfDiffLog.Model;
using System;

namespace HodStudio.EfDiffLog.TestsDotNetCore.Model
{
    [LoggedEntity]
    public class Department
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime EffectiveFrom { get; set; }
        public decimal Budget { get; set; }
    }
}
