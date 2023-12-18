using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IRegisterService
    {
        Task<bool> IsRegistered(User user);
    }
}
