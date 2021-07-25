using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace com.allcard.institution.common
{
    public class memberVM
    {
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
    }
}
