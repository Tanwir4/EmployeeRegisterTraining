using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface ITrainingRepository
    {
       Task<List<Training>> GetAllForEmployee();
        Task<List<Training>> GetAllForAdmin();
        Task<List<string>> GetAllPreRequisites();
       Task<List<Training>> GetTrainingById(int id);
       Task<bool> UpdateTraining(Training training, Department department, List<string> checkedPrerequisites);
       Task<bool> DeleteTraining(int id);
       bool AddTraining(Training training, Department department);
       Task<List<string>> GetPrerequisitesByTrainingId(int trainingID);
       Task<bool> IsTrainingApplied(int trainingId);
       int GetNewTrainingId(Training training, Department department);
    }
}
