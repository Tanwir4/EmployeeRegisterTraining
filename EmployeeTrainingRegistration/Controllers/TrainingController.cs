using DataAccessLayer.DTO;
using EmployeeTrainingRegistration.Custom;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace EmployeeTrainingRegistration.Controllers
{
    [UserSession]
    [ValidationFilter]
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IAutomaticProcessingService _automaticProcessingService;
        public TrainingController(ITrainingService trainingService, IAutomaticProcessingService automaticProcessingService)
        {
            _trainingService = trainingService;
            _automaticProcessingService = automaticProcessingService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminViewTraining()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetTraining()
        {
            List<Training> GetTraining =await _trainingService.GetAllTrainingForEmployeeAsync();
            return Json(new { trainings = GetTraining }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllTrainingForAdmin()
        {
            List<Training> GetTraining =await _trainingService.GetAllTrainingForAdminAsync();
            return Json(new { trainings = GetTraining}, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> IsTrainingApplied(int trainingId)
        {
            bool isApplied =await _trainingService.IsTrainingAppliedAsync(trainingId);
            return Json(isApplied, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> IsTrainingExpired(int trainingId)
        {
            bool isExpired = await _trainingService.IsTrainingExpired(trainingId);
            return Json(isExpired, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllPreRequisites()
        {
            List<string> allPreRequisites = new List<string>();
            allPreRequisites =await _trainingService.GetAllPreRequisitesAsync();

            return Json(new {allPreRequisite = allPreRequisites }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetTrainingById(int trainingId)
        {
         
            Training getTrainingByIdList = await _trainingService.GetAllTrainingByIdAsync(trainingId);
            Training getTrainingById = getTrainingByIdList;
            List<string> preRequisite= new List<string>();
            preRequisite =await _trainingService.GetPrerequisitesByTrainingIdAsync(trainingId);

            List<string> allPreRequisite = new List<string>();
            allPreRequisite =await _trainingService.GetAllPreRequisitesAsync();

            Session["TrainingId"] = getTrainingById;
            return Json(new { trainings = getTrainingById, preRequisites= preRequisite, allPreRequisites= allPreRequisite }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateTraining(Training training,Department department, List<string> checkedPrerequisites)
        {
            return await _trainingService.IsTrainingUpdatedAsync(training, department, checkedPrerequisites)? RedirectToAction("Login", "Login"): RedirectToAction("Register", "Register");
        }
        [HttpPost]
        public async Task<JsonResult> DeleteTraining(int id)
        {
            return await _trainingService.IsTrainingDeletedAsync(id)? Json(new { success = true, message = "Training deleted successfully." }): Json(new { success = false, message = "Training cannot be deleted." });
        }
        [HttpPost]
        public ActionResult AddTraining(Training training, Department department)
        {
            if (_trainingService.IsTrainingAdded(training, department)) { return RedirectToAction("AdminViewTraining", "Training"); }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> StartAutomaticProcessing()
        {
            List<EnrolledNotificationDTO> enrolledEmployeeList =await _automaticProcessingService.StartAutomaticProcessingAsync();
                return Json(new { success = true, message = "Automatic processing started successfully." });
        }
    }
}