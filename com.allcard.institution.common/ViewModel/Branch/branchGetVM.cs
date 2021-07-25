using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class branchGetVM
    {
        public int BranchID { get; set; }
        public int MerchantID { get; set; }

        public Guid InstitutionID { get; set; }
    }
}
