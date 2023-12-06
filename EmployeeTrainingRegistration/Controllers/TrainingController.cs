using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        // GET: Training
        public ActionResult Index()
        {
            List<Training> GetTraining = _trainingService.GetAllTraining();
            return View(GetTraining);
        }
    }
}