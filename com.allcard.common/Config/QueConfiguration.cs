using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.common
{
    public class QueConfiguration
    {
        public string Secret { get; set; }
        public int SeniorAge { get; set; }
        public int OTPExipryMinutes { get; set; }

        public string EmailUser { get; set; }
        public string EmailPassword { get; set; }
        public string EmailSMPTHost { get; set; }
        public int EmailSMPTPort { get; set; }

        public string OTPRedirect { get; set; }
        public string email_template { get; set; }

        public string TokenExpirationHours { get; set; }

        public string AllowRegions { get; set; }

        //public const int SENIOR_AGE = 60;
        //public const int OTP_EXPIRY_MINUTES = 6;
        //public const string EMAIL_USER = "OPEN";
        //public const string EMAIL_PASSWORD = "FULL";
        //public const string EMAIL_SMPT_HOST = "smtp.gmail.com";
        //public const int EMAIL_SMPT_PORT = 587;
    }
}
