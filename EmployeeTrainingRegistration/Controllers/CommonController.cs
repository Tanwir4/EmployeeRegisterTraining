using System.Web.Mvc;
namespace EmployeeTrainingRegistration.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InternalServerError()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}