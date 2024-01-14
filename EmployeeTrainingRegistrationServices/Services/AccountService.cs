using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Validation.IValidation;
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
        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> GetManagerEmailByApplicantIDAsync()
        {
            return await _userRepository.GetManagerEmailByApplicantIDAsync();
        }

        public async Task<List<string>> GetManagersByDepartmentAsync(string department)
        {
            return await _userRepository.GetAllManagersByDepartmentAsync(department);
        }

        public async  Task<int> GetUserAccountIdAsync(string email)
        {
            return await _userRepository.GetUserAccountIdByEmailAsync(email);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _userRepository.IsEmailUniqueAsync(email);
        }
    }
}
