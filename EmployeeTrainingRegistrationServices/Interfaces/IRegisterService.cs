using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IRegisterService
    {
        Task<bool> IsRegisteredAsync(User user);
    }
}
