using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Collections.Generic;
namespace EmployeeTrainingRegistrationServices.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;
        public TrainingService(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }

        public List<string> GetAllPreRequisites()
        {
            return _trainingRepository.GetAllPreRequisites();
        }

        public List<Training> GetAllTraining()
        {
            return _trainingRepository.GetAll();
        }
        public List<Training> GetAllTrainingById(int id)
        {
            return _trainingRepository.GetTrainingById(id);
        }

        public List<string> GetPrerequisitesByTrainingId(int id)
        {
            return _trainingRepository.GetPrerequisitesByTrainingId(id);
        }

        public bool IsTrainingAdded(Training training, Department department)
        {
            return _trainingRepository.AddTraining(training, department);
        }

        public bool IsTrainingDeleted(int id)
        {
            return _trainingRepository.DeleteTraining(id);
        }

        public bool IsTrainingUpdated(Training training, Department department, List<string> checkedPrerequisites)
        {
            return _trainingRepository.UpdateTraining(training, department, checkedPrerequisites);
        }
    }
}
