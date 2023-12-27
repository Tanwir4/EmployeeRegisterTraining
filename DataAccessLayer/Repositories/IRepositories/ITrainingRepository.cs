using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
namespace DataAccessLayer.Repositories.IRepositories
{
    public interface ITrainingRepository
    {
       List<Training> GetAll();
       List<Training> GetTrainingById(int id);
       bool UpdateTraining(Training training);
       bool DeleteTraining(int id);
       bool AddTraining(Training training, Department department);
    }
}
