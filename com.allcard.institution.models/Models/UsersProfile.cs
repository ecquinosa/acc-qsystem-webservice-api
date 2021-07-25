using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.allcard.institution.models
{
    public class UsersProfile : BaseEntity
    {
        [Required]
        [StringLength(16)]
        public string Username { get; set; }
        [Required]
        [StringLength(16)]
        public string Password { get; set; }

        [Required]
        [StringLength(300)]
        public string DisplayName { get; set; }


        [Required]
        public int Status { get; set; }

        public int BranchID { get; set; }
        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }

    }
}
