using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IUserRepository
    {
        bool Authenticate(Account user);
        Task<bool> Register(User user);
        int GetUserAccountIdByEmail(string email);
        int GetUserId();
        string GetRoleByEmail(string email);
    }
}
