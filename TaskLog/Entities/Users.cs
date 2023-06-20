using System;
using System.Collections.Generic;

namespace TaskLog.Entities
{
    public partial class Users
    {
        public Users()
        {
            EventLog = new HashSet<EventLog>();
            Tasks = new HashSet<Tasks>();
        }

        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public byte[] HashedPass { get; set; }

        public virtual ICollection<EventLog> EventLog { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
