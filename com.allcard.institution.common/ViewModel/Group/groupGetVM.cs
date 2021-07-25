using com.allcard.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class groupGetVM : baseVM
    {
        public int GroupID { get; set; }
        public int ChainID { get; set; }
        public Guid InstitutionID { get; set; }
    }
}
