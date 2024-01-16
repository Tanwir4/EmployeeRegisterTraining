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
        bool IsEmailUniqueAsync(string email);
        Task<bool> IsNicUniqueAsync(string nic);
        Task<bool> IsMobileNumberUniqueAsync(string mobNum);
    }
}
