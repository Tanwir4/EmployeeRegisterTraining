using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;
        public TrainingService(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }

        public async Task<List<string>> GetAllPreRequisitesAsync()
        {
            return await _trainingRepository.GetAllPreRequisitesAsync();
        }

        public async Task<List<Training>> GetAllTrainingForEmployeeAsync()
        {
            return await _trainingRepository.GetAllForEmployeeAsync();
        }
        public async Task<Training> GetAllTrainingByIdAsync(int id)
        {
            return await _trainingRepository.GetTrainingByIdAsync(id);
        }

        public async Task<List<string>> GetPrerequisitesByTrainingIdAsync(int id)
        {
            return await _trainingRepository.GetPrerequisitesByTrainingIdAsync(id);
        }

        public bool IsTrainingAdded(Training training, Department department)
        {
            return _trainingRepository.AddTraining(training, department);
        }

        public async Task<bool> IsTrainingAppliedAsync(int trainingId)
        {
            return await _trainingRepository.IsTrainingAppliedAsync(trainingId);
        }
        public async Task<bool> IsTrainingExpired(int trainingId)
        {
            return await _trainingRepository.IsTrainingExpired(trainingId);
        }

        public async Task<bool> IsTrainingDeletedAsync(int id)
        {
            return await _trainingRepository.DeleteTrainingAsync(id);
        }

        public async Task<bool> IsTrainingUpdatedAsync(Training training, Department department, List<string> checkedPrerequisites)
        {
            return await _trainingRepository.UpdateTrainingAsync(training, department, checkedPrerequisites);
        }

        public async Task<List<Training>> GetAllTrainingForAdminAsync()
        {
            return await _trainingRepository.GetAllForAdminAsync();
        }
    }
}
