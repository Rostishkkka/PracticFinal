using System;
using System.Collections.Generic;

namespace TaskLog.Entities
{
    public partial class Components
    {
        public Components()
        {
            Tasks = new HashSet<Tasks>();
        }

        public long CompId { get; set; }
        public string? CompOemId { get; set; }
        public string? CompOemVer { get; set; }
        public string CompOemName { get; set; }
        public string? SwVer { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
