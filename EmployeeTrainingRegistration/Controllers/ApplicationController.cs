using EmployeeTrainingRegistrationServices.Interfaces;
using System.IO;
using System.Web;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Services;
using DataAccessLayer.Models;

namespace EmployeeTrainingRegistration.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubmitApplication(HttpPostedFileBase fileInput)
        {
            if (fileInput != null && fileInput.ContentLength > 0)
            {
                int trainingId = Convert.ToInt32(Request.Form["trainingId"]);
                Console.WriteLine($"Received trainingId: {trainingId}");
                // Convert file data to byte array
                byte[] fileData;
                using (var binaryReader = new BinaryReader(fileInput.InputStream))
                {
                    fileData = binaryReader.ReadBytes(fileInput.ContentLength);
                }

                // Save application details and file data
                if (_applicationService.IsApplicationSubmitted(trainingId, fileData))
                {
                    return Json(new { success = true, message = "Application Submitted" });
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                // Handle case when no file is uploaded
                return Json(new { success = false, message = "No file uploaded" });
            }
        }

        [HttpGet]
        public JsonResult GetApplicationById()
        {
            List<UserApplication> getApplicationById = _applicationService.GetApplicationDetailsByUserId();
            return Json(new { applications = getApplicationById }, JsonRequestBehavior.AllowGet);
        }
    }
}