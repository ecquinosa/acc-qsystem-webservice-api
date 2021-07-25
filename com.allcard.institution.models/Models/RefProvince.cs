using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.models
{
    public class RefProvince : BaseEntity
    {
        public string PSGCRegionCode { get; set; }
        public string PSGCProvinceCode { get; set; }
        public string PSGCProvinceDescription { get; set; }

    }
}
