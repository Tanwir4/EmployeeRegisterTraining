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
        public ActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> IsMobileUnique(string mobileNumber)
        {
            bool isMobileNumberUnique = await _accountService.IsMobileNumberUniqueAsync(mobileNumber);
            return Json(!isMobileNumberUnique? new { success = true, message = "Registration successful!" }: new { success = false, message = "Mobile number already exists!" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> IsNicUnique(string nic)
        {
            bool isNicUnique = await _accountService.IsNicUniqueAsync(nic);
            return Json(!isNicUnique? new { success = true, message = "Registration successful!" }: new { success = false, message = "NIC number already exists!" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult IsEmailUnique(string email)
       {
            bool isEmailUnique =  _accountService.IsEmailUniqueAsync(email);
            return Json(!isEmailUnique? new { success = true, message = "Registration successful!" }: new { success = false, message = "Email already exists!" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> AddAccount(User user)
        {
            return await _registerService.IsRegisteredAsync(user)? RedirectToAction("Login", "Login"): (ActionResult)View("Error");
        }
        [HttpGet]
        public async Task<JsonResult> ManagersByDepartment(string department)
        {
            var Getmanagers = await _accountService.GetManagersByDepartmentAsync(department);
            return Json(new { managers = Getmanagers }, JsonRequestBehavior.AllowGet);
        }
    }
}