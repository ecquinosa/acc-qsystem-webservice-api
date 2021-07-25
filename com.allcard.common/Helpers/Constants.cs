using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.common
{
    public static class Constants
    {
        public const string RESULT_CODE_SUCCESS = "200";
        public const string RESULT_CODE_VALIDATION_FAILED = "304";
        //public const string RESULT_CODE_SERVER_ERROR = "504";
        public const string RESULT_CODE_SERVER_ERROR = "201";

        public const string STATUS_OPEN = "OPEN";
        public const string STATUS_CANCELLED = "CANCELLED";
        public const string STATUS_FULL = "FULL";
        public const string STATUS_FINISH = "FINISH";

        public const int USER_STATUS_ACTIVE = 1;
        public const int USER_STATUS_BLOCKED = 2;

        public const string MODULE_AUTHENTICATE = "Authenticate";
        public const string MODULE_GENERATE_SCHEDULE = "Generate Schedule";
        public const string MODULE_RESERVE_SLOT = "Reserve Slot";
        public const string MODULE_CONFIRM_OTP = "Confirm OTP";

        public const string MODULE_SCAN_CODE = "Scan Code";
        public const string MODULE_SCAN_IN = "Scan In";
        public const string MODULE_SCAN_OUT = "Scan Out";
        public const string MODULE_GET_SCHEDULE = "Get Schedule";
        public const string MODULE_CANCEL_SCHEDULE = "Cancel Schedule";
    }
}
