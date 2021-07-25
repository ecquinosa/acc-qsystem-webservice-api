using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class locationGetVM
    {
        public int LocationID { get; set; }
        public int BranchID { get; set; }
        public Guid InstitutionID { get; set; }
    }

    public class locationGetDetailsVM
    {
        public Guid LocationID { get; set; }
    }
}
