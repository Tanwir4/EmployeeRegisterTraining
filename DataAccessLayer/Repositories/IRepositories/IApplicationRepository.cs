using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IApplicationRepository
    {
        Task<bool> SaveApplicationAsync(int trainingId, List<byte[]> fileDataList);
        Task<List<UserApplication>> GetApplicationDetailsByUserIdAsync();
        Task<List<ManagerApplicationDTO>> GetApplicationsByManagerIdAsync();
        Task<string> ApproveApplicationAsync(string name,string title);
        Task<bool> DeclineApplicationAsync(string name, string title,string declineReason);
        Task<EmailDTO> GetManagerApprovalDetailsAsync(int applicationId);
        Task<List<int>> GetAttachmentsByApplicationIdAsync(int applicationId);
        Task<byte[]> GetAttachmentsByIdAsync(int attachmentId);

    }
}
