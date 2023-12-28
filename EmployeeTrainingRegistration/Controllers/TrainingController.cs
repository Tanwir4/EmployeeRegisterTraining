using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace EmployeeTrainingRegistration.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        public TrainingController(ITrainingService trainingService)
        {
            _trainingService = trainingService;
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
        public JsonResult GetTraining()
        {
            List<Training> GetTraining = _trainingService.GetAllTraining();
            return Json(new { trainings = GetTraining }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllPreRequisites()
        {
            List<string> allPreRequisites = new List<string>();
            allPreRequisites = _trainingService.GetAllPreRequisites();

            return Json(new {allPreRequisite = allPreRequisites }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTrainingById(int trainingId)
        {
            Training getTrainingById = _trainingService.GetAllTrainingById(trainingId).FirstOrDefault();
            List<string> preRequisite= new List<string>();
            preRequisite = _trainingService.GetPrerequisitesByTrainingId(trainingId);

            List<string> allPreRequisite = new List<string>();
            allPreRequisite = _trainingService.GetAllPreRequisites();

            Session["TrainingId"] = getTrainingById;
            return Json(new { trainings = getTrainingById, preRequisites= preRequisite, allPreRequisites= allPreRequisite }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateTraining(Training training,Department department, List<string> checkedPrerequisites)
        {
            if (_trainingService.IsTrainingUpdated(training, department, checkedPrerequisites)) { return RedirectToAction("Login", "Login"); }
            else { return RedirectToAction("Register", "Register"); }
        }

        [HttpPost]
        public ActionResult DeleteTraining(int id)
        {
            if (_trainingService.IsTrainingDeleted(id)) { return RedirectToAction("Login", "Login"); }
            else { return RedirectToAction("Register", "Register"); }
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
    }
}