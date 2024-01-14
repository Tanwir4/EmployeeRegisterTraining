using DataAccessLayer.DTO;
using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IAutomaticProcessingRepository
    {
        Task<List<Training>> GetTrainingByDeadlineAsync();
        Task<List<EnrolledNotificationDTO>> ProcessApplicationAsync();
        Task<List<EnrolledEmployeeForExportDTO>> GetSelectedEmployeeListAsync(int id);
    }
}
