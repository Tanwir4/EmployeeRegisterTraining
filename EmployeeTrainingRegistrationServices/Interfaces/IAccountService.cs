using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IAccountService
    {
        Task<int> GetUserAccountIdAsync(string email);
        Task<string> GetManagerEmailByApplicantID();
        Task<List<string>> GetManagersByDepartment(string department);
        Task<bool> IsEmailUnique(string email);
    }
}
