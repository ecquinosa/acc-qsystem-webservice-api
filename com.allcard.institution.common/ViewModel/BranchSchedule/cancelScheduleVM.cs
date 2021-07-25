using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class cancelScheduleVM
    {
        public Guid ScheduleCode { get; set; }
        public DateTime? Date { get; set; }
        public string Message { get; set; }
    }
}
