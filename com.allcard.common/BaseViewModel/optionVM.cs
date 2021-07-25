using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.common
{
    public class optionVM
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public ICollection<optionVM> Children { get; set; }
    }

}
