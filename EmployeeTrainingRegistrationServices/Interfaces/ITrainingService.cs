using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface ITrainingService
    {
        Task<List<Training>> GetAllTrainingForEmployee();
        Task<List<Training>> GetAllTrainingForAdmin();
        //List<Training> DisplayTrainingWithPrerequisites();
        Task<List<Training>> GetAllTrainingById(int id);
        Task<bool> IsTrainingUpdated(Training training, Department department, List<string> checkedPrerequisites);
        Task<bool> IsTrainingDeleted(int id);
        bool IsTrainingAdded(Training training, Department department);
        Task<List<string>> GetPrerequisitesByTrainingId(int id);
        Task<List<string>> GetAllPreRequisites();
        Task<bool> IsTrainingApplied(int trainingId);
    }
}
