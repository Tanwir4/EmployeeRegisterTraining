using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<Account> AuthenticateAsync(Account account);
        Task<bool> Register(User user);
        Task<int> GetUserAccountIdByEmailAsync(string email);
        Task<int> GetUserIdAsync();
        Task <string> GetRoleByEmailAsync(string email);
        Task<string> GetManagerEmailByApplicantID();
        Task<List<string>> GetAllManagersByDepartment(string department);
        Task<bool> IsEmailUnique(string email);
    }
}
