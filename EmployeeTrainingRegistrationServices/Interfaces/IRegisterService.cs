using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IRegisterService
    {
        bool CheckRegister(UserDetails user, UserAccount acc, Department dept);
    }
}
