using EmployeeTrainingRegistrationServices.Entities;
using System.Collections.Generic;
namespace EmployeeTrainingRegistrationServices.Interfaces
{
    public interface ITrainingService
    {
        List<Training> GetAllTraining();
    }
}
