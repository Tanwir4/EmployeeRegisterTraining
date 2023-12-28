using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface ITrainingService
    {
        List<Training> GetAllTraining();
        //List<Training> DisplayTrainingWithPrerequisites();
        List<Training> GetAllTrainingById(int id);
        bool IsTrainingUpdated(Training training, Department department, List<string> checkedPrerequisites);
        bool IsTrainingDeleted(int id);
        bool IsTrainingAdded(Training training, Department department);
        List<string> GetPrerequisitesByTrainingId(int id);
        List<string> GetAllPreRequisites();
    }
}
