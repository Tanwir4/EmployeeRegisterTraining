using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface ILoginService
    {
        bool VerifyLogin(UserAccount acc);
    }
}
