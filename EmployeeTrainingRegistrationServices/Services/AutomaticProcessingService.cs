using DataAccessLayer.DTO;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Services
{
    public class AutomaticProcessingService : IAutomaticProcessingService
    {
        private readonly IAutomaticProcessingRepository _automaticProcessingRepository;
        public AutomaticProcessingService(IAutomaticProcessingRepository automaticProcessingRepository)
        {
            _automaticProcessingRepository = automaticProcessingRepository;
        }

        public List<EnrolledNotificationDTO> StartAutomaticProcessing()
        {
            return _automaticProcessingRepository.ProcessApplication();
        }
    }
}
