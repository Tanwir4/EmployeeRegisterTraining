using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IAccountService
    {
        Task<int> GetUserAccountIdAsync(string email);
        Task<string> GetManagerEmailByApplicantID();
    }
}
