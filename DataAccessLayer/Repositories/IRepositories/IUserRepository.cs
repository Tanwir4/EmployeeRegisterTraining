using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> AuthenticateAsync(Account user);
        Task<bool> Register(User user);
        Task<int> GetUserAccountIdByEmailAsync(string email);
        Task<int> GetUserIdAsync();
        Task <string> GetRoleByEmailAsync(string email);
        Task<string> GetManagerEmailByApplicantID();
    }
}
