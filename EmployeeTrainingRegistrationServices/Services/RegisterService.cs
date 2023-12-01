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
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        public RegisterService(IUserRepository userRepository) 
        { 
            _userRepository = userRepository;
        }
        public bool CheckRegister(UserDetails user, UserAccount acc, Department dept)
        {
            if (_userRepository.RegisterUser(user,acc,dept)) { return true; }
            else { return false; }
        }
    }
}
