using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IApplicationService
    {
        Task<bool> IsApplicationSubmitted(int trainingId, List<byte[]> fileDataList);
        Task<List<UserApplication>> GetApplicationDetailsByUserId();
        Task<List<ManagerApplicationDTO>> GetApplicationByManagerId();
        Task<string> IsApplicationApproved(string name, string title);
        Task<bool> IsApplicationDeclined(string name, string title,string declineReason);
        Task<EmailDTO> GetManagerApprovalDetails(int applicationId);
        Task<List<int>> GetAttachmentsByApplicationId(int applicationId);
        Task<byte[]> GetAttachmentsById(int attachmentId);
    }
}
