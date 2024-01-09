using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Validation.IValidation;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginValidation _loginValidation;
        public LoginService(IUserRepository userRepository, ILoginValidation loginValidation)
        {
            _userRepository = userRepository;
            _loginValidation = loginValidation;
        }
        public async  Task<bool> IsAuthenticatedAsync(Account account)
        {
            return await _userRepository.AuthenticateAsync(account);
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
