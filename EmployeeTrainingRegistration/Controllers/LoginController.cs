using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Web.Mvc;

namespace EmployeeTrainingRegistration.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            if (_loginService.IsAuthenticated(acc)) { 
                //return View("Success");
                return RedirectToAction("Index", "Training");
                }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password."; 
                return View("Login", acc);
            }
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}