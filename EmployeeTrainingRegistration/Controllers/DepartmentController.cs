using EmployeeTrainingRegistration.Custom;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetAllDepartmentName()
        {
                List<Department> GetDepartmentName =await _departmentService.GetAllDepartmentNameAsync();
                return Json(new { departments = GetDepartmentName }, JsonRequestBehavior.AllowGet);
        }
    }
}