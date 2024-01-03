using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;


namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IApplicationService
    {
        bool IsApplicationSubmitted(int trainingId, List<byte[]> fileDataList);
        List<UserApplication> GetApplicationDetailsByUserId();
        List<ManagerApplicationDTO> GetApplicationByManagerId();
        List<DocumentDTO> GetDocuments(int userID, int trainingID, int applicationID);
        string IsApplicationApproved(string name, string title);
        bool IsApplicationDeclined(string name, string title,string declineReason);
        EmailDTO GetManagerApprovalDetails(int applicationId);
    }
}
