using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.allcard.institution.models
{
    public class Branch : BaseEntity
    {
        [Required]
        [StringLength(60)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        public bool Status { get; set; }


        [Required]
        public DateTime StoreHoursOpen { get; set; }
        [Required]
        public DateTime StoreHoursClose { get; set; }

        [Required]
        public int DefaultPersonCount { get; set; }
        [Required]
        public int DefaultIntervalSecond { get; set; }

        public bool IsGenerateSchedule { get; set; }

        #region Relation
        public int MerchantID { get; set; }
        [ForeignKey("MerchantID")]
        public virtual Merchant Merchant { get; set; }

        public Guid InstitutionID { get; set; }

        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<UsersProfile> UsersProfile { get; set; }

        #endregion
    }
}
