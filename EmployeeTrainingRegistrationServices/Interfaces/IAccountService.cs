using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IAccountService
    {
        Task<int> GetUserAccountIdAsync(string email);
        Task<string> GetManagerEmailByApplicantIDAsync();
        Task<List<string>> GetManagersByDepartmentAsync(string department);
        Task<bool> IsEmailUniqueAsync(string email);
    }
}
