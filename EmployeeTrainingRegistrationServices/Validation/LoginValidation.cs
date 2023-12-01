using EmployeeTrainingRegistrationServices.Validation.IValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Validation
{
    public class LoginValidation : ILoginValidation
    {
        public List<string> ValidationLoginResults(string emailAddress, string password)
        {
            List<string> results = new List<string>();
            Regex EmailRegex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (string.IsNullOrEmpty(emailAddress))
                results.Add(("Please enter an email address."));
            if (string.IsNullOrEmpty(password))
                results.Add(("Please enter a password."));
            if ((!string.IsNullOrEmpty(emailAddress)) && (!EmailRegex.IsMatch(emailAddress)))
                results.Add(("Invalid Email Address!"));
            return results;
        }
    }
}
