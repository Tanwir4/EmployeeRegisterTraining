using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Validation.IValidation;
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
        public bool IsAuthenticated(Account account)
        {
            return _userRepository.Authenticate(account);
        }

        public int GetUserIdByEmail(string email)
        {
            return _userRepository.GetUserAccountIdByEmail(email);
        }
    }
}
