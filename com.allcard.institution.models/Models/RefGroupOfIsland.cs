using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.models
{
    public class RefGroupOfIsland : BaseEntity
    {
        public string IslandGroup { get; set; }
        public virtual ICollection<RefRegion> RefRegion { get; set; }
    }
}
