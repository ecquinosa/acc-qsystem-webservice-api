using com.allcard.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class branchScheduleMemberVM : baseVM
    {
        public string RefNumber { get; set; }

        public bool IsSenior { get; set; }

        public DateTime OTPExpiry { get; set; }

     
        public int MemberID { get; set; }
        public string Member { get; set; }

        public DateTime TimeIn { get; set; }

        public DateTime TimeOut { get; set; }

        public int BranchScheduleID { get; set; }
    }
}
