using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
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

        public async  Task<List<ManagerApplicationDTO>> GetApplicationByManagerId()
        {
            return await _applicationRepository.GetApplicationsByManagerId();
        }

        public async Task<EmailDTO> GetManagerApprovalDetails(int applicationId) {
            return await _applicationRepository.GetManagerApprovalDetails(applicationId);
        }

        public async Task<List<UserApplication>> GetApplicationDetailsByUserId()
        {
           return await _applicationRepository.GetApplicationDetailsByUserId();
        }

        public async Task<string> IsApplicationApproved(string name, string title)
        {
            return await _applicationRepository.ApproveApplication( name,title);
        }

        public async Task<bool> IsApplicationDeclined(string name, string title, string declineReason)
        {
            return await _applicationRepository.DeclineApplication(name, title, declineReason);
        }

        public async Task<bool> IsApplicationSubmitted(int trainingId,List<byte[]> fileDataList)
        {
            return await _applicationRepository.saveApplication(trainingId, fileDataList);
        }

        public async Task<List<int>> GetAttachmentsByApplicationId(int applicationId)
        {
            return await _applicationRepository.GetAttachmentsByApplicationId(applicationId);
        }

        public async Task<byte[]> GetAttachmentsById(int attachmentId)
        {
            return await _applicationRepository.GetAttachmentsById(attachmentId);
        }
    }
}
