using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IAccountService
    {
        bool AuthenticateUser(UserAccount acc);
        bool RegisterUser(UserDetails user, UserAccount acc, Department dept);
    }
}
