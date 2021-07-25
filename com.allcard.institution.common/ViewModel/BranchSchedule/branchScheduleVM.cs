using com.allcard.common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace com.allcard.institution.common
{
    public class branchScheduleVM : baseVM
    {
        [Required]
        [StringLength(60)]
        public string Status { get; set; }
        //[Required]
        //public DateTime StoreOpen { get; set; }
        //[Required]
        //public DateTime StoreClose { get; set; }
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

        public int AvailableSlot { get; set; }

        public int VerifiedCount { get; set; }

        [Required]
        [StringLength(300)]
        public string Remarks { get; set; }


        #region Relation
        public int BranchID { get; set; }

        public string Branch { get; set; }

        public Guid InstitutionID { get; set; }
        #endregion
    }
}
