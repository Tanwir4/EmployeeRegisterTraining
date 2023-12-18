using DataAccessLayer.Models;
namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface ILoginService
    {
        bool IsAuthenticated(Account acc);
        int GetUserIdByEmail(string email);
    }
}
