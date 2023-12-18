using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
namespace EmployeeTrainingRegistrationServices.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        public RegisterService(IUserRepository userRepository) 
        { 
            _userRepository = userRepository;
        }
        public bool IsRegistered(User user)
        {
            return _userRepository.Register(user);
        }
    }
}
