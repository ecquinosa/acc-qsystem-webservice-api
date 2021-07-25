using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.common
{
    public class paginationVM
    {
        public object List { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
    }
}
