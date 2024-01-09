using DataAccessLayer.DTO;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTrainingRegistration.Controllers
{
    public class AutomaticProcessingController : Controller
    {
        private readonly IAutomaticProcessingService _automaticProcessingService;
        public AutomaticProcessingController(IAutomaticProcessingService automaticProcessingService)
        {
            _automaticProcessingService = automaticProcessingService;
        }
        // GET: AutomaticProcessing
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetSelectedEmployeeByTrainingId(int id)
        {
            List<EnrolledEmployeeForExportDTO> selectedEmployee = new List<EnrolledEmployeeForExportDTO>();
            selectedEmployee = await _automaticProcessingService.GetSelectedEmployeeList(id);
            return Json(new { selectedEmployees = selectedEmployee }, JsonRequestBehavior.AllowGet);

        }

    }
}