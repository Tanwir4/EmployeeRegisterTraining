using EmployeeTrainingRegistrationServices.Interfaces;
using System.IO;
using System.Web;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using DataAccessLayer.Models;
using System.Threading.Tasks;
using DataAccessLayer.DTO;

namespace EmployeeTrainingRegistration.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly INotificationService _notificationService;

        public ApplicationController(IApplicationService applicationService, INotificationService notificationService)
        {
            _applicationService = applicationService;
            _notificationService = notificationService;
        }
        public ActionResult Index()
        {
            return View();
        }
        /*        [HttpPost]
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
        */

        [HttpPost]
        public async Task<ActionResult> SubmitApplication(int trainingId, List<HttpPostedFileBase> fileInputs)
        {
            try
            {
                // Ensure that trainingId is not zero (or any default value)
                if (trainingId <= 0)
                {
                    return Json(new { success = false, message = "Invalid trainingId" });
                }

                List<byte[]> fileDataList = new List<byte[]>();

                // Loop through each file and convert file data to byte array
                foreach (var fileInput in fileInputs)
                {
                    if (fileInput != null && fileInput.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(fileInput.InputStream))
                        {
                            byte[] fileData = binaryReader.ReadBytes(fileInput.ContentLength);
                            fileDataList.Add(fileData);
                        }
                    }
                }

                // Save application details and file data
                if (await _applicationService.IsApplicationSubmitted(trainingId, fileDataList))
                {
                    
                    return Json(new { success = true, message = "Applications Submitted" });
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetApplicationById()
        {
            List<UserApplication> getApplicationById =await _applicationService.GetApplicationDetailsByUserId();
            return Json(new { applications = getApplicationById }, JsonRequestBehavior.AllowGet);
        }


    }
}