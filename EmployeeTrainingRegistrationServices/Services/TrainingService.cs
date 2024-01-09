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

        public async Task<List<string>> GetAllPreRequisites()
        {
            return await _trainingRepository.GetAllPreRequisites();
        }

        public async Task<List<Training>> GetAllTrainingForEmployee()
        {
            return await _trainingRepository.GetAllForEmployee();
        }
        public async Task<List<Training>> GetAllTrainingById(int id)
        {
            return await _trainingRepository.GetTrainingById(id);
        }

        public async Task<List<string>> GetPrerequisitesByTrainingId(int id)
        {
            return await _trainingRepository.GetPrerequisitesByTrainingId(id);
        }

        public bool IsTrainingAdded(Training training, Department department)
        {
            return _trainingRepository.AddTraining(training, department);
        }

        public async Task<bool> IsTrainingApplied(int trainingId)
        {
            return await _trainingRepository.IsTrainingApplied(trainingId);
        }

        public async Task<bool> IsTrainingDeleted(int id)
        {
            return await _trainingRepository.DeleteTraining(id);
        }

        public async Task<bool> IsTrainingUpdated(Training training, Department department, List<string> checkedPrerequisites)
        {
            return await _trainingRepository.UpdateTraining(training, department, checkedPrerequisites);
        }

        public async Task<List<Training>> GetAllTrainingForAdmin()
        {
            return await _trainingRepository.GetAllForAdmin();
        }
    }
}
