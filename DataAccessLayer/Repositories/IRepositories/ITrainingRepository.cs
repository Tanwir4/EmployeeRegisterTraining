using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface ITrainingRepository
    {
       Task<List<Training>> GetAllForEmployeeAsync();
        Task<List<Training>> GetAllForAdminAsync();
        Task<List<string>> GetAllPreRequisitesAsync();
       Task<Training> GetTrainingByIdAsync(int id);
       Task<bool> UpdateTrainingAsync(Training training, Department department, List<string> checkedPrerequisites);
       Task<bool> DeleteTrainingAsync(int id);
       bool AddTraining(Training training, Department department);
       Task<List<string>> GetPrerequisitesByTrainingIdAsync(int trainingID);
       Task<bool> IsTrainingAppliedAsync(int trainingId);
       int GetNewTrainingId(Training training, Department department);
        Task<bool> DoesTrainingExistInEnrolledAsync(int id);
        Task<bool> IsTrainingExpired(int id);
    }
}
