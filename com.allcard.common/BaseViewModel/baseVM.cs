using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.common
{
    public class baseVM
    {
        public int ID { get; set; }
        public Guid GUID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
