using System.Collections.Generic;

namespace EmployeeTrainingRegistrationServices.Validation.IValidation
{
    public interface ILoginValidation
    {
        List<string> ValidationLoginResults(string emailAddress, string password);
    }
}
