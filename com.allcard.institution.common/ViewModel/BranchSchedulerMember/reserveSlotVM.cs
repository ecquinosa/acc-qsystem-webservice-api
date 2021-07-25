using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.institution.common.ViewModel
{
    public class reserveSlotVM
    {
        public Guid ScheduleCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string BirthPlaceCode { get; set; }
        public string CCANo { get; set; }

        public string CIF { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        public string TransactionType { get; set; }

        public string RedirectURL { get; set; }
    }
}
