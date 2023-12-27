using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface ITrainingService
    {
        List<Training> GetAllTraining();
        List<Training> GetAllTrainingById(int id);
        bool IsTrainingUpdated(Training training);
        bool IsTrainingDeleted(int id);
        bool IsTrainingAdded(Training training, Department department);
    }
}
