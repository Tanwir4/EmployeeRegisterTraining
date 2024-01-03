using DataAccessLayer.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class EmailNotificationService
    {
        public static string SendApprovalEmail(string recipientEmail, string trainingTitle)
        {
            string subject = "Training Application Approved";
            string body = $"Hello,\n\nYour training application for '{trainingTitle}' has been approved.";

            return EmailSender.SendEmail(subject, body, recipientEmail);
        }

        public static string SendDeclineEmail(string recipientEmail, string trainingTitle, string declineReason)
        {
            string subject = "Training Application Declined";
            string body = $"Hello,\n\nYour training application for '{trainingTitle}' has been declined. " +
                          $"Reason: {declineReason}";

            return EmailSender.SendEmail(subject, body, recipientEmail);
        }
    }
}
