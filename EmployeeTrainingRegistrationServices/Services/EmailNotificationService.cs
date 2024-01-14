using DataAccessLayer.Notifications;
using EmployeeTrainingRegistrationServices.Interfaces;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class EmailNotificationService : INotificationService
    {
        public  string SendApprovalEmail(string recipientEmail, string trainingTitle)
        {
            string subject = "Training Application Approved";
            string body = $"Hello,\n\nYour training application for '{trainingTitle}' has been approved.";

            return EmailSender.SendEmail(subject, body, recipientEmail);
        }

        public  string SendDeclineEmail(string recipientEmail, string trainingTitle, string declineReason)
        {
            string subject = "Training Application Declined";
            string body = $"Hello,\n\nYour training application for '{trainingTitle}' has been declined. " +
                          $"Reason: {declineReason}";

            return EmailSender.SendEmail(subject, body, recipientEmail);
        }

        public  string SendSelectedEmail(string recipientEmail, string trainingTitle)
        {
            string subject = "Training Enrollment Confirmation";
            string body = $"Hello,\n\nYou have been selected for '{trainingTitle}.";

            return EmailSender.SendEmail(subject, body, recipientEmail);
        }

        public string NotifyManager(string recipientEmail, string trainingTitle)
        {
            string subject = "New Training Application";
            string body = $"Hello,\n\nYou have an application to process for '{trainingTitle}'.";

            return EmailSender.SendEmail(subject, body, recipientEmail);
        }
    }
}
