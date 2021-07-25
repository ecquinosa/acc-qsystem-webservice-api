using com.allcard.common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace com.allcard.institution.common
{
    public class merchantVM : baseVM
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
        public virtual string Group { get; set; }
        public Guid InstitutionID { get; set; }
        #endregion
    }
}
