using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistration.Custom;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Verify(Account acc)
        {
            if (await _loginService.IsAuthenticatedAsync(acc)) {
                Session["Email"] = acc.Email;
                Session["UserAccountId"] =await _accountService.GetUserAccountIdAsync(acc.Email);
               
                string roleName =await _loginService.GetRoleNameByEmailAsync(acc.Email);
                Session["CurrentRole"] = roleName;
                if (roleName =="Employee") { return RedirectToAction("Index", "Training"); }
                else if(roleName == "Manager") { return RedirectToAction("Index", "Manager"); }
                else { return RedirectToAction("AdminViewTraining", "Training"); }
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