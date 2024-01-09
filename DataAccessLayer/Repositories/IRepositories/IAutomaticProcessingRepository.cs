using DataAccessLayer.DTO;
using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IAutomaticProcessingRepository
    {
        Task<List<Training>> GetTrainingByDeadline();
        Task<List<EnrolledNotificationDTO>> ProcessApplication();
        Task<List<EnrolledEmployeeForExportDTO>> GetSelectedEmployeeList(int id);
    }
}
