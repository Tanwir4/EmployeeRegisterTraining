using DataAccessLayer.Models;
using EmployeeTrainingRegistration.Custom;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace EmployeeTrainingRegistration.Controllers
{
    [ValidationFilter]
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly IAccountService _accountService;
        public RegisterController(IRegisterService registerService, IAccountService accountService)
        {
            _registerService = registerService;
            _accountService = accountService;
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> IsEmailUnique(string email)
        {
            bool isEmailUnique = await _accountService.IsEmailUnique(email);
            if (isEmailUnique)
            {
                return Json(new { error = "Registration Successful!" });
            }
            else return Json(new { error = "Email already exists." });
        }
        [HttpPost]
        public async Task<ActionResult> AddAccount(User user)
        {
            if (await _registerService.IsRegistered(user)) { return RedirectToAction("Login", "Login"); }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<JsonResult> ManagersByDepartment(string department)
        {
            var Getmanagers = await _accountService.GetManagersByDepartment(department);
            return Json(new { managers = Getmanagers }, JsonRequestBehavior.AllowGet);
        }


    }
}