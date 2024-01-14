using DataAccessLayer.Models;
using EmployeeTrainingRegistration.Custom;
using EmployeeTrainingRegistration.Enums;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace EmployeeTrainingRegistration.Controllers
{
    [ValidationFilter]
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
        public async Task<ActionResult> Verify(Account acc)
        {
            if (await _loginService.IsAuthenticatedAsync(acc))
            {
                Session["Email"] = acc.Email;
                Session["UserAccountId"] = await _accountService.GetUserAccountIdAsync(acc.Email);
                string roleName = await _loginService.GetRoleNameByEmailAsync(acc.Email);
                Session["CurrentRole"] = roleName;
                Enum.TryParse(roleName, out Role UserRole);
                if (UserRole == Role.Employee){return RedirectToAction("Index", "Training");}
                else if (UserRole == Role.Manager) { return RedirectToAction("Index", "Manager"); }
                else if (UserRole == Role.Admin) { return RedirectToAction("AdminViewTraining", "Training"); }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("Login", acc);
            }
            return View("Login", acc);
        }
    }
}