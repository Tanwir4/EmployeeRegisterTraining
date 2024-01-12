using EmployeeTrainingRegistration.Custom;
using System.Web.Mvc;

namespace EmployeeTrainingRegistration.Controllers
{
    [UserSession]
    public class LogOutController : Controller
    {
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        // GET: Logout
        public ActionResult Index()
        {
            Session.Clear();
            return RedirectToAction("Login", "Login");
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return View();
        }
    }
}