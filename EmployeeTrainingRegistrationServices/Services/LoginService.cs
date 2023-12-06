using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Validation.IValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
        public bool IsAuthenticated(Account account)
        {
            return _userRepository.Authenticate(account);
        }
    }
}
