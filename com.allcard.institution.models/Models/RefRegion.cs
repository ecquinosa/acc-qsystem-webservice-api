using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.allcard.institution.models
{
    public class RefRegion : BaseEntity
    {
        public string PSGCRegionCode { get; set; }
        public string PSGCRegionDescription { get; set; }

        public int groupOfIslandID { get; set; }
        [ForeignKey("groupOfIslandID")]
        public virtual RefGroupOfIsland RefGroupOfIsland { get; set; }
    }
}
