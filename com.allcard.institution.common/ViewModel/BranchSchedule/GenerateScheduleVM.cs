using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common.ViewModel
{
    public class generateScheduleVM
    {
        public DateTime Date { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsSenior { get; set; }
        public Guid BranchCode { get; set; }

        public int PersonCount { get; set; }
        public int HoursCount { get; set; }
    }
}
