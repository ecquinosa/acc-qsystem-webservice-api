using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace com.allcard.common
{
    public static class MailClient
    {
        public static IConfiguration _config { get; set; }
        //public async static void Send(string subject,string email, string message, QueConfiguration config)
        //{
            
        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient(config.EmailSMPTHost);

        //    mail.From = new MailAddress(config.EmailUser);
        //    mail.To.Add(email);
        //    mail.Subject = subject;
        //    mail.Body = message;
        //    mail.IsBodyHtml = true;

        //    SmtpServer.UseDefaultCredentials = true;
        //    SmtpServer.Port = config.EmailSMPTPort;
        //    SmtpServer.Credentials = new System.Net.NetworkCredential(config.EmailUser, config.EmailPassword);

        //    await SmtpServer.SendMailAsync(mail);

        //}
        public async static void Send(string subject, string email, string message, QueConfiguration config)
        {

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(string.Empty, config.EmailUser));
            mimeMessage.To.Add(new MailboxAddress(string.Empty, email));
            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(config.EmailSMPTHost, config.EmailSMPTPort, true);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(config.EmailUser, config.EmailPassword);

                await client.SendAsync(mimeMessage);
                client.Disconnect(true);
            }
        }

        public static void SendConfirm(string subject,string email,string reserveCode ,string pin, string redirect,string description, QueConfiguration config)
        {
           
            string body = string.Empty;
            string templatePath = Directory.GetCurrentDirectory() + config.email_template;
            using (StreamReader reader = new StreamReader(templatePath))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{title}", "Hello"); //replacing the required things  
            body = body.Replace("{description1}", description);
            body = body.Replace("{confirmlink}", string.Format("{0}?pin={1}&code={2}", redirect, pin, reserveCode));
            body = body.Replace("{description2}", "");

            MailClient.Send(subject,email, body, config);
        }

        public static void SendCancellation(string subject, string email,string store, string description1, string remarks, QueConfiguration config)
        {

            string body = string.Empty;
            string templatePath = Directory.GetCurrentDirectory() + "\\Template\\email_cancel_template.html";
            using (StreamReader reader = new StreamReader(templatePath))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{title}", string.Format("Reservation @ <b>{0}</b> has been cancelled",store)); //replacing the required things  
            body = body.Replace("{description1}", description1);
            body = body.Replace("{description2}", string.Format("{0}", remarks));


            MailClient.Send(subject,email, body, config);
        }
    }
}
