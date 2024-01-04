using DataAccessLayer.AppLogger;
using DataAccessLayer.DBConnection;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Services;
using EmployeeTrainingRegistrationServices.Validation;
using EmployeeTrainingRegistrationServices.Validation.IValidation;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
namespace EmployeeTrainingRegistration
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IDataAccessLayer, DataAccessLayer.DBConnection.DataAccessLayer>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IAutomaticProcessingRepository, AutomaticProcessingRepository>();
            container.RegisterType<ITrainingRepository, TrainingRepository>();
            container.RegisterType<IApplicationRepository, ApplicationRepository>();
            container.RegisterType<IDepartmentRepository, DepartmentRepository>();
            container.RegisterType<ILoginService, LoginService>();
            container.RegisterType<ITrainingService, TrainingService>();
            container.RegisterType<IAutomaticProcessingService, AutomaticProcessingService>();
            container.RegisterType<ILoginValidation, LoginValidation>();
            container.RegisterType<IRegisterService, RegisterService>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<IApplicationService, ApplicationService>();
            container.RegisterType<ILogger, Logger>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}