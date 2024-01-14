using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IApplicationService
    {
        Task<bool> IsApplicationSubmittedAsync(int trainingId, List<byte[]> fileDataList);
        Task<List<UserApplication>> GetApplicationDetailsByUserIdAsync();
        Task<List<ManagerApplicationDTO>> GetApplicationByManagerIdAsync();
        Task<string> IsApplicationApprovedAsync(string name, string title);
        Task<bool> IsApplicationDeclinedAsync(string name, string title,string declineReason);
        Task<EmailDTO> GetManagerApprovalDetailsAsync(int applicationId);
        Task<List<int>> GetAttachmentsByApplicationIdAsync(int applicationId);
        Task<byte[]> GetAttachmentsByIdAsync(int attachmentId);
    }
}
