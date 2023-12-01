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
        bool AuthenticateUser(UserAccount user);
        bool RegisterUser(UserDetails user, UserAccount acc, Department dept);
    }
}
