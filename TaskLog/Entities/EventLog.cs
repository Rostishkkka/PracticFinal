using System;
using System.Collections.Generic;

namespace TaskLog.Entities
{
    public partial class EventLog
    {
        public long EventId { get; set; }
        public string EventType { get; set; }
        public DateTime EventTimestamp { get; set; }
        public long TaskId { get; set; }
        public long UserId { get; set; }

        public virtual Tasks Task { get; set; }
        public virtual Users User { get; set; }
    }
}
