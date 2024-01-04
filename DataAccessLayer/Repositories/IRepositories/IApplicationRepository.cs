using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IApplicationRepository
    {
        bool saveApplication(int trainingId, List<byte[]> fileDataList);
        List<UserApplication> GetApplicationDetailsByUserId();
        List<ManagerApplicationDTO> GetApplicationsByManagerId();
        string ApproveApplication(string name,string title);
        bool DeclineApplication(string name, string title,string declineReason);
        EmailDTO GetManagerApprovalDetails(int applicationId);
        List<int> GetAttachmentsByApplicationId(int applicationId);
        byte[] GetAttachmentsById(int attachmentId);


    }
}
