using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }
        public bool IsApplicationSubmitted(int trainingId, byte[] fileData)
        {
            return _applicationRepository.saveApplication(trainingId, fileData);
        }
    }
}
