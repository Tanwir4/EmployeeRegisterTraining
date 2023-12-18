using DataAccessLayer.Models;
namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IUserRepository
    {
        bool Authenticate(Account user);
        bool Register(User user);
        int GetUserAccountIdByEmail(string email);
        int GetUserId();
    }
}
