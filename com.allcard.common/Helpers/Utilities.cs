using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.allcard.common
{
    public static class Utilities
    {
        public static string GetTimestamp(DateTime uTCdateTime)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcNow = uTCdateTime;
            TimeSpan elapsedTime = utcNow - unixEpoch;
            double millis = elapsedTime.TotalMilliseconds;
            return millis.ToString();
        }

        public static long GetTimestampToInt(DateTime uTCdateTime)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcNow = uTCdateTime;
            TimeSpan elapsedTime = utcNow - unixEpoch;
            long result = (long)elapsedTime.TotalMilliseconds;
            return result;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
