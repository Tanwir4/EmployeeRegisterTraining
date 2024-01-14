using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface ITrainingService
    {
        Task<List<Training>> GetAllTrainingForEmployeeAsync();
        Task<List<Training>> GetAllTrainingForAdminAsync();
        //List<Training> DisplayTrainingWithPrerequisites();
        Task<Training> GetAllTrainingByIdAsync(int id);
        Task<bool> IsTrainingUpdatedAsync(Training training, Department department, List<string> checkedPrerequisites);
        Task<bool> IsTrainingDeletedAsync(int id);
        bool IsTrainingAdded(Training training, Department department);
        Task<List<string>> GetPrerequisitesByTrainingIdAsync(int id);
        Task<List<string>> GetAllPreRequisitesAsync();
        Task<bool> IsTrainingAppliedAsync(int trainingId);
        Task<bool> IsTrainingExpired(int id);
    }
}
