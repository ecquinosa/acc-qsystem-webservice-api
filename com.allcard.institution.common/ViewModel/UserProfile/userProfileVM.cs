using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class userProfileVM
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string DisplayName { get; set; }
        public int Status { get; set; }

        public int BranchID { get; set; }
        public virtual string Branch { get; set; }

    }
}
