using DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface IAutomaticProcessingService
    {
        Task<List<EnrolledNotificationDTO>> StartAutomaticProcessingAsync();
        Task<List<EnrolledEmployeeForExportDTO>> GetSelectedEmployeeListAsync(int id);
    }
}
