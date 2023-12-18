using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
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
        public async Task<bool> IsRegistered(User user)
        {
            return await _userRepository.Register(user);
        }
    }
}
