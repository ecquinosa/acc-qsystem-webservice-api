using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class updateScheduleVM
    {
        public Guid ScheduleCode { get; set; }
        public int MaxPersonCount { get; set; }
        public bool IsSenior { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }
    }
}
