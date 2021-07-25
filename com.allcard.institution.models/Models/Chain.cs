using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace com.allcard.institution.models
{
    public class Chain : BaseEntity
    {
        [Required]
        [StringLength(60)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        public bool Status { get; set; }

        #region Relation
        public Guid InstitutionID { get; set; }
        //public virtual Institution Institution { get; set; }

        public virtual ICollection<Group> Group { get; set; }
        #endregion
    }
}
