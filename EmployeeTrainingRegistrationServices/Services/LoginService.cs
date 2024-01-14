using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Security;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        
        }
        public async  Task<bool> IsAuthenticatedAsync(Account account)
        {

           Account acc= await _userRepository.AuthenticateAsync(account);
            if(acc != null && PasswordHashing.VerifyPassword(account.Password,acc.HashedPassword,acc.Salt)) { return true; }
            return false;
        }

        public async Task<int> GetUserIdByEmailAsync(string email)
        {
            return await _userRepository.GetUserAccountIdByEmailAsync(email);
        }

        public async Task<string> GetRoleNameByEmailAsync(string email)
        {
            return await _userRepository.GetRoleByEmailAsync(email);
        }
    }
}
