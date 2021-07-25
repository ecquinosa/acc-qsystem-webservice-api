using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common
{
    public class scanCodeResponseVM
    {
        public memberVM Member { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
    }
}
