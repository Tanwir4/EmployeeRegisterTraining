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
        public ActionResult AddTraining()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetTraining()
        {
            List<Training> GetTraining =await _trainingService.GetAllTrainingForEmployee();
            return Json(new { trainings = GetTraining }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllTrainingForAdmin()
        {
            List<Training> GetTraining =await _trainingService.GetAllTrainingForAdmin();
            return Json(new { trainings = GetTraining }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> IsTrainingApplied(int trainingId)
        {
            bool isApplied =await _trainingService.IsTrainingApplied(trainingId);
            return Json(isApplied, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllPreRequisites()
        {
            List<string> allPreRequisites = new List<string>();
            allPreRequisites =await _trainingService.GetAllPreRequisites();

            return Json(new {allPreRequisite = allPreRequisites }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetTrainingById(int trainingId)
        {
         
            Training getTrainingByIdList = await _trainingService.GetAllTrainingById(trainingId);
            Training getTrainingById = getTrainingByIdList;
            List<string> preRequisite= new List<string>();
            preRequisite =await _trainingService.GetPrerequisitesByTrainingId(trainingId);

            List<string> allPreRequisite = new List<string>();
            allPreRequisite =await _trainingService.GetAllPreRequisites();

            Session["TrainingId"] = getTrainingById;
            return Json(new { trainings = getTrainingById, preRequisites= preRequisite, allPreRequisites= allPreRequisite }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTraining(Training training,Department department, List<string> checkedPrerequisites)
        {
            if (await _trainingService.IsTrainingUpdated(training, department, checkedPrerequisites)) { return RedirectToAction("Login", "Login"); }
            else { return RedirectToAction("Register", "Register"); }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteTraining(int id)
        {
            if (await _trainingService.IsTrainingDeleted(id)) { return Json(new { success = true, message = "Training deleted successfully." }); }
            else { return Json(new { success = false, message = "Training cannot be deleted." }); }
        }

        [HttpPost]
        public ActionResult AddTraining(Training training, Department department)
        {
            if (_trainingService.IsTrainingAdded(training, department)) { return RedirectToAction("Login", "Login"); }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> StartAutomaticProcessing()
        {
            List<EnrolledNotificationDTO> enrolledEmployeeList =await _automaticProcessingService.StartAutomaticProcessing();
                return Json(new { success = true, message = "Automatic processing started successfully." });
        }
    }
}