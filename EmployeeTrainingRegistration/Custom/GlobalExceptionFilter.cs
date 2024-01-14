using DataAccessLayer.AppLogger;
using System.Web.Mvc;

namespace EmployeeTrainingRegistration.Custom
{
    public class GlobalExceptionFilter : HandleErrorAttribute
    {
        private readonly ILogger _logger;
        public GlobalExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            _logger.LogError(filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.Result = new ViewResult()
            {
                ViewName = "InternalServerError",
                TempData = filterContext.Controller.TempData
            };
        }
    }
}