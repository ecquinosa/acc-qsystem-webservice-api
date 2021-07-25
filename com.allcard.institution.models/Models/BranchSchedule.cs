using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.allcard.institution.models
{
    public class BranchSchedule : BaseEntity
    {

        [Required]
        [StringLength(60)]
        public string Status { get; set; }
        [Required]
        public DateTime StoreOpen { get; set; }
        [Required]
        public DateTime StoreClose { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int MaxPersonCount { get; set; }
        [Required]
        public bool IsSenior { get; set; }

        [Required]
        [StringLength(300)]
        public string Remarks { get; set; }


        #region Relation
        public int BranchID { get; set; }
        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }

        public Guid InstitutionID { get; set; }

      

        #endregion

    }

}
