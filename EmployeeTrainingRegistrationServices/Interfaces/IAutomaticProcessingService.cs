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
        Task<List<EnrolledNotificationDTO>> StartAutomaticProcessing();
        Task<List<EnrolledEmployeeForExportDTO>> GetSelectedEmployeeList(int id);
    }
}
