using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using EmployeeTrainingRegistration.Custom;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Xml.Linq;

namespace EmployeeTrainingRegistration.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IApplicationService _applicationService;
        public ManagerController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        // GET: Manager
        [CustomAuthorization("Manager")]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorization("Manager")]
        [HttpGet]
        public JsonResult GetApplicationsByManagerId()
        {
            List<ManagerApplicationDTO> getApplicationsByManager = _applicationService.GetApplicationByManagerId();
            return Json(new { applications = getApplicationsByManager }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ApproveApplication(string name, string title,int applicationID)
        {
            string success= _applicationService.IsApplicationApproved(name, title);
            EmailDTO emailDetails = _applicationService.GetManagerApprovalDetails(applicationID);
            string applicantEmail = emailDetails.EmployeeEmail;
            EmailNotificationService.SendApprovalEmail(applicantEmail, emailDetails.TrainingTitle);
            if (success=="Approved")
            {
                return Json(new { success = true, message = "Application approved successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to approve application" });
            }
        }

        [HttpPost]
        public ActionResult DeclineApplication(string name, string title,string declineReason,int applicationID)
        {
            bool success = _applicationService.IsApplicationDeclined(name, title, declineReason);
            EmailDTO emailDetails = _applicationService.GetManagerApprovalDetails(applicationID);
            string applicantEmail = emailDetails.EmployeeEmail;
            EmailNotificationService.SendDeclineEmail(applicantEmail, emailDetails.TrainingTitle, declineReason);
            if (success)
            {
                return Json(new { success = true, message = "Application declined successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to decline application" });
            }
        }
        [HttpGet]
        public ActionResult GetAttachmentsByApplicationID(int applicationID)
        {
            List<int> getAttachments = _applicationService.GetAttachmentsByApplicationId(applicationID);



            return Json(new { success = true, Attachments= getAttachments }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public ActionResult DownloadAttachment(int attachmentID)
        {

            byte[] binaryFile = _applicationService.GetAttachmentsById(attachmentID);

            string contentType = "application/octet-stream";

            string fileName = Uri.UnescapeDataString("hello"); // Decode encoded file name

            return  File(binaryFile, contentType, fileName);

        }

    }
}