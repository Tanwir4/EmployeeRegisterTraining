using DataAccessLayer.Models;
namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IRegisterService
    {
        bool IsRegistered(User user);
    }
}
