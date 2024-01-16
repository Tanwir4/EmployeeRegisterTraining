using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Collections.Generic;
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

        public bool IsEmailUniqueAsync(string email)
        {
            return  _userRepository.IsEmailUniqueAsync(email);
        }
        public async Task<bool> IsNicUniqueAsync(string nic)
        {
            return await _userRepository.IsNicUniqueAsync(nic);
        }
        public async Task<bool> IsMobileNumberUniqueAsync(string mobNum)
        {
            return await _userRepository.IsMobileNumberUniqueAsync(mobNum);
        }
    }
}
