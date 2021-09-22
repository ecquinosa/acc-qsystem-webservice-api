using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace com.allcard.institution.models
{
    public class Member : BaseEntity
    {
        [Required]
        [StringLength(14)]
        public string CCANo { get; set; }

        [Required]
        [StringLength(13)]
        public string CIF { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

       [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
                
        [StringLength(10)]
        public string Suffix { get; set; }

        [Required]
        [StringLength(60)]
        public string FullName { get; set; }

        [Required]
        [StringLength(20)]
        public string MobileNumber { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        [StringLength(60)]
        public string BirthPlace { get; set; }

        [Required]
        [StringLength(60)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; }
    }
}
