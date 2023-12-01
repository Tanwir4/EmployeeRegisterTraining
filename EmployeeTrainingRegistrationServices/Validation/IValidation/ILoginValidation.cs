using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Validation.IValidation
{
    public interface ILoginValidation
    {
       List<string> ValidationLoginResults(string emailAddress, string password);
    }
}
