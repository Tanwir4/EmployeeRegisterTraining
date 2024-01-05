using DataAccessLayer.AppLogger;
using EmployeeTrainingRegistration.Custom;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTrainingRegistration
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            var logger = DependencyResolver.Current.GetService<ILogger>();
            filters.Add(new GlobalExceptionFilter(logger));
        }
    }
}
