using DataAccessLayer.DTO;
using EmployeeTrainingRegistration.Custom;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace EmployeeTrainingRegistration.Controllers
{
    [UserSession]
    public class AutomaticProcessingController : Controller
    {
        private readonly IAutomaticProcessingService _automaticProcessingService;
        public AutomaticProcessingController(IAutomaticProcessingService automaticProcessingService)
        {
            _automaticProcessingService = automaticProcessingService;
        }
        [CustomAuthorization("Admin")]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorization("Admin")]
        [HttpGet]
        public async Task<JsonResult> GetSelectedEmployeeByTrainingId(int id)
        {
            List<EnrolledEmployeeForExportDTO> selectedEmployee= await _automaticProcessingService.GetSelectedEmployeeListAsync(id);
            return Json(new { selectedEmployees = selectedEmployee }, JsonRequestBehavior.AllowGet);
        }

    }
}