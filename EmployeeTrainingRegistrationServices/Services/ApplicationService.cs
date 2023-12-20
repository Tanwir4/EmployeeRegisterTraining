using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
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

        public List<UserApplication> GetApplicationDetailsByUserId()
        {
           return _applicationRepository.GetApplicationDetailsByUserId();
        }

        public bool IsApplicationSubmitted(int trainingId, byte[] fileData)
        {
            return _applicationRepository.saveApplication(trainingId, fileData);
        }
    }
}
