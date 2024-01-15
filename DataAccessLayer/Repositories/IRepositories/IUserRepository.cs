using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<Account> AuthenticateAsync(Account account);
        Task<bool> RegisterAsync(User user);
        Task<int> GetUserAccountIdByEmailAsync(string email);
        Task<int> GetUserIdAsync();
        Task <string> GetRoleByEmailAsync(string email);
        Task<string> GetManagerEmailByApplicantIDAsync();
        Task<List<string>> GetAllManagersByDepartmentAsync(string department);
        bool IsEmailUniqueAsync(string email);
    }
}
