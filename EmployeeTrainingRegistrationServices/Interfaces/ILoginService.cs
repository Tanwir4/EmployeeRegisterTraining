using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface ILoginService
    {
        Task<bool> IsAuthenticatedAsync(Account acc);
        Task<int> GetUserIdByEmailAsync(string email);
        Task<string> GetRoleNameByEmailAsync(string email);
    }
}
