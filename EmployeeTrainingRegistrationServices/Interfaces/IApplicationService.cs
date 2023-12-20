using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IApplicationService
    {
        bool IsApplicationSubmitted(int trainingId, byte[] fileData);
        List<UserApplication> GetApplicationDetailsByUserId();
    }
}
