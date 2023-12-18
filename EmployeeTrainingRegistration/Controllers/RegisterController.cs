using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace EmployeeTrainingRegistration.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
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
        [HttpPost]
        public async Task<ActionResult> AddAccount(User user)
        {
            if (await _registerService.IsRegistered(user)) {return RedirectToAction("Login","Login"); }
            else {
                return View("Error");
            }
        }
    }
}