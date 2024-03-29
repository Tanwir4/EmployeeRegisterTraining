﻿using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<Account> AuthenticateAsync(Account account);
        Task<bool> RegisterAsync(User user);
        Task<int> GetUserAccountIdByEmailAsync(string email);
        Task<int> GetUserIdAsync();
        Task<string> GetRoleByEmailAsync(string email);
        Task<string> GetManagerEmailByApplicantIDAsync();
        Task<List<string>> GetAllManagersByDepartmentAsync(string department);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsNicUniqueAsync(string nic);
        Task<bool> IsMobileNumberUniqueAsync(string mobNum);
        Task<bool> GetValueByColumnAsync<T>(string tableName, string columnName, string value);
    }
}
