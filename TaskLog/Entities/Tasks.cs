using System;
using System.Collections.Generic;

namespace TaskLog.Entities
{
    public partial class Tasks
    {
        public Tasks()
        {
            EventLog = new HashSet<EventLog>();
        }

        public long TaskId { get; set; }
        public long CompId { get; set; }
        public string TaskDescr { get; set; }
        public string CompSn { get; set; }
        public long UserId { get; set; }
        public string? Comments { get; set; }

        public virtual Components Comp { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<EventLog> EventLog { get; set; }
    }
}
