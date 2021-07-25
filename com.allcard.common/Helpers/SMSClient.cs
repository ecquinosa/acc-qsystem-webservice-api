using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace com.allcard.common
{
    public static class SMSClient
    {

        public static void Send(string MobileNo, string Message)
        {
            string URI = $"https://sms12.synermaxx.asia/vmobile/snappycab/api/sendnow.php?username=snappyapi&password=323387e8cf0efbfcccc5ac97923ebe7d&mobilenum={MobileNo}&fullmesg={Message}&originator=SNAPPY&company=snappycab";
            var request = (HttpWebRequest)WebRequest.Create(URI);
            var response = (HttpWebResponse)request.GetResponse();

            //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}
