using EmployeeTrainingRegistrationServices.Interfaces;
using System.Web.Mvc;
namespace EmployeeTrainingRegistration.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IAccountService _accountService;
        public ApplicationController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubmitApplication(int trainingId)
        {
            if (_accountService.IsApplicationSubmitted(trainingId)) { return Json(new { success = true, message = "Application Submitted" }); }
            else
            {
                return View("Error");
            }
        }
    }
}