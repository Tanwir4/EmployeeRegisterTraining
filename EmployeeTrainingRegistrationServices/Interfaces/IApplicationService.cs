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
        string IsApplicationApproved(string name, string title);
        bool IsApplicationDeclined(string name, string title,string declineReason);
        EmailDTO GetManagerApprovalDetails(int applicationId);
        List<int> GetAttachmentsByApplicationId(int applicationId);
        byte[] GetAttachmentsById(int attachmentId);
    }
}
