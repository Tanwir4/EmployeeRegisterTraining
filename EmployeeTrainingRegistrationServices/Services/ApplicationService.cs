using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
using System.Collections.Generic;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public List<ManagerApplicationDTO> GetApplicationByManagerId()
        {
            return _applicationRepository.GetApplicationsByManagerId();
        }

        public EmailDTO GetManagerApprovalDetails(int applicationId) {
            return _applicationRepository.GetManagerApprovalDetails(applicationId);
        }

        public List<UserApplication> GetApplicationDetailsByUserId()
        {
           return _applicationRepository.GetApplicationDetailsByUserId();
        }

        public List<DocumentDTO> GetDocuments(int userID, int trainingID, int applicationID)
        {
            return _applicationRepository.GetDocumentsByApplicationID(userID, trainingID, applicationID);
        }

        public string IsApplicationApproved(string name, string title)
        {
            return _applicationRepository.ApproveApplication( name,title);
        }

        public bool IsApplicationDeclined(string name, string title, string declineReason)
        {
            return _applicationRepository.DeclineApplication(name, title, declineReason);
        }

        public bool IsApplicationSubmitted(int trainingId,List<byte[]> fileDataList)
        {
            return _applicationRepository.saveApplication(trainingId, fileDataList);
        }
    }
}
