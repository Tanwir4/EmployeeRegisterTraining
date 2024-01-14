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
        private readonly INotificationService _notificationService;

        public AutomaticProcessingService(IAutomaticProcessingRepository automaticProcessingRepository,INotificationService notificationService)
        {
            _automaticProcessingRepository = automaticProcessingRepository;
            _notificationService = notificationService;
        }

        public async Task<List<EnrolledEmployeeForExportDTO>> GetSelectedEmployeeListAsync(int id)
        {
            return await _automaticProcessingRepository.GetSelectedEmployeeListAsync(id);
        }

        public async Task<List<EnrolledNotificationDTO>> StartAutomaticProcessingAsync()
        {
            var enrolledEmployeeList = await _automaticProcessingRepository.ProcessApplicationAsync();
            foreach (var enrolledEmployees in enrolledEmployeeList)
            {
                _notificationService.SendSelectedEmail(enrolledEmployees.Email, enrolledEmployees.Title);
            }
            return enrolledEmployeeList;
        }
    }
}
