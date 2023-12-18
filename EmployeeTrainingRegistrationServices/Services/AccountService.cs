using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.IRepositories;
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
        private readonly IApplicationRepository _applicationRepository;
        public AccountService(IUserRepository userRepository, IApplicationRepository applicationRepository)
        {
            _userRepository = userRepository;
            _applicationRepository = applicationRepository;
        }
        public int GetUserAccountId(string email)
        {
            return _userRepository.GetUserAccountIdByEmail(email);
        }

        public bool IsApplicationSubmitted(int trainingId)
        {
            return _applicationRepository.submitApplication(trainingId);
        }
    }
}
