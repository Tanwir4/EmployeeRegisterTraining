using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async  Task<List<ManagerApplicationDTO>> GetApplicationByManagerIdAsync()
        {
            return await _applicationRepository.GetApplicationsByManagerIdAsync();
        }

        public async Task<EmailDTO> GetManagerApprovalDetailsAsync(int applicationId) {
            return await _applicationRepository.GetManagerApprovalDetailsAsync(applicationId);
        }

        public async Task<List<UserApplication>> GetApplicationDetailsByUserIdAsync()
        {
           return await _applicationRepository.GetApplicationDetailsByUserIdAsync();
        }

        public async Task<string> IsApplicationApprovedAsync(string name, string title)
        {
            return await _applicationRepository.ApproveApplicationAsync( name,title);
        }

        public async Task<bool> IsApplicationDeclinedAsync(string name, string title, string declineReason)
        {
            return await _applicationRepository.DeclineApplicationAsync(name, title, declineReason);
        }

        public async Task<bool> IsApplicationSubmittedAsync(int trainingId, List<byte[]> fileDataList)
        {
            return await _applicationRepository.SaveApplicationAsync(trainingId, fileDataList);
        }


        public async Task<List<int>> GetAttachmentsByApplicationIdAsync(int applicationId)
        {
            return await _applicationRepository.GetAttachmentsByApplicationIdAsync(applicationId);
        }

        public async Task<byte[]> GetAttachmentsByIdAsync(int attachmentId)
        {
            return await _applicationRepository.GetAttachmentsByIdAsync(attachmentId);
        }
    }
}
