using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class getBranchScheduleVM
    {
        public Guid branchCode { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public int page { get; set; }
        public int row { get; set; }
    }
}
