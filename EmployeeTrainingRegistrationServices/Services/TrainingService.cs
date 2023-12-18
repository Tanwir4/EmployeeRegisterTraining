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
        public List<Training> GetAllTraining()
        {
            return _trainingRepository.GetAll();
        }
        public List<Training> GetAllTrainingById(int id)
        {
            return _trainingRepository.GetTrainingById(id);
        }
    }
}
