using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class branchScheduleBoardVM
    {
        public bool IsSenior { get; set; }
        public string Status { get; set; }
        public Guid ScheduleCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalSlots { get; set; }
        public int AvailableSlot { get; set; }
        public int VerifiedCount { get; set; }
    }
}
