using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface INotificationService
    {
         string SendSelectedEmail(string recipientEmail, string trainingTitle);
        string SendApprovalEmail(string recipientEmail, string trainingTitle);
        string SendDeclineEmail(string recipientEmail, string trainingTitle, string declineReason);
        string NotifyManager(string recipientEmail, string trainingTitle, string applicantName);

    }
}
