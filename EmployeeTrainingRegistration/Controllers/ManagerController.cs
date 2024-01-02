using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Interfaces;
using System.Collections.Generic;
using System.Web.Mvc;

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
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetApplicationsByManagerId()
        {
            List<ManagerApplicationDTO> getApplicationsByManager = _applicationService.GetApplicationByManagerId();
            return Json(new { applications = getApplicationsByManager }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDocuments(int userID, int trainingID, int applicationID)
        {
            List<DocumentDTO> documents = _applicationService.GetDocuments(userID, trainingID, applicationID);
            return Json(new { documents }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ApproveApplication(string name, string title)
        {
            string success= _applicationService.IsApplicationApproved(name, title);
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
        public ActionResult DeclineApplication(string name, string title,string declineReason)
        {
            bool success = _applicationService.IsApplicationDeclined(name, title, declineReason);
            if (success)
            {
                return Json(new { success = true, message = "Application declined successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to decline application" });
            }
        }
    }
}