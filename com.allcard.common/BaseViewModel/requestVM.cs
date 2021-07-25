using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.common
{
    public class requestVM
    {
        public Guid JTI { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public DateTime Expiration { get; set; }
        public object Data { get; set; }
    }
}
