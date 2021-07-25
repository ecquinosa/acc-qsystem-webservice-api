using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.allcard.institution.models
{
    public class UserRole : BaseEntity
    {
        #region Relation
        public int UserProfileID { get; set; }
        [ForeignKey("UserProfileID")]
        public virtual UsersProfile UsersProfile { get; set; }


        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }

        #endregion
    }
}
