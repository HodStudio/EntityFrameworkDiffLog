using System;

namespace HodStudio.EfDiffLog.Model
{
    public class LogEntry
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string Operation { get; set; }
        public DateTime LogDateTime { get; set; }
        public string ValuesChanges { get; set; }
    }
}
