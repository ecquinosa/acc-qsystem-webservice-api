using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.allcard.institution.models
{
    public class BranchScheduleMember : BaseEntity
    {
       
        [StringLength(30)]
        public string RefNumber { get; set; }

        public bool IsSenior { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerifiedDate { get; set; }

        [Required]
        [StringLength(6)]
        public string OTP { get; set; }
        [Required]
        public DateTime OTPExpiry { get; set; }

        
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }

        #region Foreign

        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public virtual Member Member { get; set; }
        

        public int BranchScheduleID { get; set; }
        [ForeignKey("BranchScheduleID")]
        public virtual BranchSchedule BranchSchedule { get; set; }

        #endregion

    }
}
