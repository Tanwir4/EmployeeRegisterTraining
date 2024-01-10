using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IApplicationRepository
    {
        Task<bool> saveApplication(int trainingId, List<byte[]> fileDataList);
        Task<List<UserApplication>> GetApplicationDetailsByUserId();
        Task<List<ManagerApplicationDTO>> GetApplicationsByManagerId();
        Task<string> ApproveApplication(string name,string title);
        Task<bool> DeclineApplication(string name, string title,string declineReason);
        Task<EmailDTO> GetManagerApprovalDetails(int applicationId);
        Task<List<int>> GetAttachmentsByApplicationId(int applicationId);
        Task<byte[]> GetAttachmentsById(int attachmentId);
        //Task<EmailDTO> GetManagerDetailsForNotification(int applicationId);


    }
}
