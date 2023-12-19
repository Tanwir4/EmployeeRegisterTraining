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
        //private readonly IApplicationService _applicationService;
        public TrainingController(ITrainingService trainingService) 
        {
            _trainingService = trainingService;
            //_applicationService= applicationService;
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
        [HttpPost]
        public ActionResult UploadFile()
        {
            try
            {
                var file = Request.Files[0];

                // Save the file to the database or perform any other necessary actions
                // Replace this with your database storage logic

                return Json(new { success = true, message = "File uploaded successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error uploading file: " + ex.Message });
            }
        }
    }
}