﻿using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Security;
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
        public async Task<bool> IsRegisteredAsync(User user)
        {
            user.Salt = PasswordHashing.GenerateSalt();

            user.HashedPassword = PasswordHashing.ComputeStringToSha256Hash(user.Password, user.Salt);
            return await _userRepository.RegisterAsync(user);
        }
    }
}
