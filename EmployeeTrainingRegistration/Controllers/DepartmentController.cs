using EmployeeTrainingRegistration.Custom;
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
    [UserSession]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetAllDepartmentName()
        {
            try
            {
                List<Department> GetDepartmentName =await _departmentService.GetAllDepartmentName();
                return Json(new { departments = GetDepartmentName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetAllDepartmentName: {ex.Message}");
                return Json(new { error = "Internal Server Error" });
            }
        }
    }
}