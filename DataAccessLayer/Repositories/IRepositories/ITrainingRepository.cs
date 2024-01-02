using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
namespace DataAccessLayer.Repositories.IRepositories
{
    public interface ITrainingRepository
    {
       List<Training> GetAll();
       List<string> GetAllPreRequisites();
       List<Training> GetTrainingById(int id);
       bool UpdateTraining(Training training, Department department, List<string> checkedPrerequisites);
       bool DeleteTraining(int id);
       bool AddTraining(Training training, Department department);
       List<string> GetPrerequisitesByTrainingId(int trainingID);
        //List<Training> GetAllWithPrerequisites();

        int GetNewTrainingId(Training training, Department department);
    }
}
