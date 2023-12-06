using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        public AccountService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }
        public bool AuthenticateUser(UserAccount acc)
        {
            if (_userRepository.AuthenticateUser(acc)) { return true; }
            else { return false; }
        }
        public bool RegisterUser(UserDetails user, UserAccount acc, Department dept)
        {
            if (_userRepository.RegisterUser(user, acc, dept)) { return true; }
            else { return false; }
        }
    }
}
