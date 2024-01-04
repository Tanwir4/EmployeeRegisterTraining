using DataAccessLayer.DTO;
using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;


namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IAutomaticProcessingRepository
    {
        List<Training> GetTrainingByDeadline();
        List<EnrolledNotificationDTO> ProcessApplication();
    }
}
