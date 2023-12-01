using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IUserRepository
    {
    /*    bool AddNewUser(); //Register
        UserAccount GetAccountById(int id);
        UserAccount GetAccountByEmail(string email);*/
        bool GetUserLogin(UserAccount user);
        bool AddUserAccount(UserDetails user, UserAccount acc, Department dept);
 

    }
}
