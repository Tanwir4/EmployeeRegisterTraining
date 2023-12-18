using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Web.Mvc;
namespace EmployeeTrainingRegistration.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IAccountService _accountService;
        public LoginController(ILoginService loginService, IAccountService accountService)
        {
            _loginService = loginService;
            _accountService = accountService;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            if (_loginService.IsAuthenticated(acc)) {
                Session["Email"] = acc.Email;
                Session["UserAccountId"] = _accountService.GetUserAccountId(acc.Email);
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