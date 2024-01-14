using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using EmployeeTrainingRegistration.Custom;
using EmployeeTrainingRegistration.Enums;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmployeeTrainingRegistration.Controllers
{
    [UserSession]
    public class ManagerController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly INotificationService _notificationService;
        public ManagerController(IApplicationService applicationService, INotificationService notificationService)
        {
            _applicationService = applicationService;
            _notificationService = notificationService;
        }
        [CustomAuthorization("Manager")]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorization("Manager")]
        [HttpGet]
        public async Task<JsonResult> GetApplicationsByManagerId()
        {
            List<ManagerApplicationDTO> getApplicationsByManager =await _applicationService.GetApplicationByManagerIdAsync();
            return Json(new { applications = getApplicationsByManager }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> ApproveApplication(string name, string title,int applicationID)
        {
            string success=await _applicationService.IsApplicationApprovedAsync(name, title);
            EmailDTO emailDetails =await _applicationService.GetManagerApprovalDetailsAsync(applicationID);
            string applicantEmail = emailDetails.EmployeeEmail;
            _notificationService.SendApprovalEmail(applicantEmail, emailDetails.TrainingTitle);
            Enum.TryParse(success, out ApplicationStatus status);
            if (status == ApplicationStatus.Approved){return Json(new { success = true, message = "Application approved successfully" });}
            else if (status == ApplicationStatus.Declined){return Json(new { success = false, message = "Failed to approve application" });}
            return Json(new { success = false, message = "An error occurred while processing the application." });
        }
        [HttpPost]
        public async Task<ActionResult> DeclineApplication(string name, string title,string declineReason,int applicationID)
        {
            bool success =await _applicationService.IsApplicationDeclinedAsync(name, title, declineReason);
            EmailDTO emailDetails =await _applicationService.GetManagerApprovalDetailsAsync(applicationID);
            string applicantEmail = emailDetails.EmployeeEmail;
            _notificationService.SendDeclineEmail(applicantEmail, emailDetails.TrainingTitle, declineReason);
            if (success){return Json(new { success = true, message = "Application declined successfully" });}
            else{return Json(new { success = false, message = "Failed to decline application" });}
        }
        [HttpGet]
        public async Task<ActionResult> GetAttachmentsByApplicationID(int applicationID)
        {
            List<int> getAttachments =await _applicationService.GetAttachmentsByApplicationIdAsync(applicationID);
            return Json(new { success = true, Attachments= getAttachments }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> DownloadAttachment(int attachmentID)
        {
            byte[] binaryFile =await _applicationService.GetAttachmentsByIdAsync(attachmentID);
            string contentType = "application/octet-stream";
            string fileName = Uri.UnescapeDataString("hello");
            return  File(binaryFile, contentType, fileName);
        }
    }
}