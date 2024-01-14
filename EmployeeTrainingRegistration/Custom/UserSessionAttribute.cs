using System.Web.Mvc;

namespace EmployeeTrainingRegistration.Custom
{
    public class UserSessionAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserAccountId"] == null || filterContext.HttpContext.Session["CurrentRole"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Login");
            }
        }
    }
}