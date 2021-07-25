using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace com.allcard.institution.models
{
    public class Institution : BaseEntity
    {
        [Required]
        [StringLength(60)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
