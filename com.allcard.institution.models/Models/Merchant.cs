using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.allcard.institution.models
{
    public class Merchant: BaseEntity
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
        public int GroupID { get; set; }
        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }

        public virtual ICollection<Branch> Branch { get; set; }

        public Guid InstitutionID { get; set; }
        #endregion
    }
}
