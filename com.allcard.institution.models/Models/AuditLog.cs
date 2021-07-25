using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace com.allcard.institution.models
{
    public class AuditLog : BaseEntity
    {
        [Required]
        [StringLength(60)]
        public string Module { get; set; }
        [Required]
        [StringLength(400)]
        public string Description { get; set; }

    }
}
