using EmployeeTrainingRegistrationServices.Interfaces;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using DataAccessLayer.Models;
using System.Threading.Tasks;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistration.Custom;

namespace EmployeeTrainingRegistration.Controllers
{
    [UserSession]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;
        private readonly ITrainingService _trainingService;

        public ApplicationController(IApplicationService applicationService, INotificationService notificationService, IAccountService accountService,ITrainingService trainingService)
        {
            _applicationService = applicationService;
            _notificationService = notificationService;
            _accountService = accountService;
            _trainingService = trainingService;
        }
        [CustomAuthorization("Employee")]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorization("Employee")]
        [HttpPost]
        public async Task<ActionResult> SubmitApplication(int trainingId, List<HttpPostedFileBase> fileInputs)
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
                if (await _applicationService.IsApplicationSubmittedAsync(trainingId, fileDataList))
                {

                    string managerEmail = await _accountService.GetManagerEmailByApplicantIDAsync();
                    Training training= await _trainingService.GetAllTrainingByIdAsync(trainingId);
                   _notificationService.NotifyManager(managerEmail, training.Title);
                    return Json(new { success = true, message = "Applications Submitted" });
                }
                else
                {
                    return View("Error");
                }
        }

        [HttpGet]
        public async Task<JsonResult> GetApplicationById()
        {
            List<UserApplication> getApplicationById =await _applicationService.GetApplicationDetailsByUserIdAsync();
            return Json(new { applications = getApplicationById }, JsonRequestBehavior.AllowGet);
        }
    }
}