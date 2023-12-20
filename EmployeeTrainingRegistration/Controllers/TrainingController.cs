using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}