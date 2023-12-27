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
        public JsonResult GetTrainingById(int trainingId)
        {
            Training getTrainingById = _trainingService.GetAllTrainingById(trainingId).FirstOrDefault();
            Session["TrainingId"] = getTrainingById;
            return Json(new { trainings = getTrainingById }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateTraining(Training training)
        {
            if(_trainingService.IsTrainingUpdated(training)) { return RedirectToAction("Login", "Login"); }
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