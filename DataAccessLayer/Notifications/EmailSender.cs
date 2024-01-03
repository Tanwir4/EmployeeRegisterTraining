using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Notifications
{
    public class EmailSender
    {
        public static string SendEmail(string Subject, string Body, string recipientEmail)
        {
            string senderMail = "tanwirbibinasweerah.lollmohamud@ceridian.com";

            var smtpClient = new SmtpClient("relay.ceridian.com")
            {
                Port = 25,
                EnableSsl = true,
                UseDefaultCredentials = true
            };

            var mailMessage = new MailMessage(senderMail, recipientEmail)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            };
            try
            {
                smtpClient.Send(mailMessage);
                return "Email sent successfully!";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
