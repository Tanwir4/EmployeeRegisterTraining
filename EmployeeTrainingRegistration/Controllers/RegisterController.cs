using DataAccessLayer.Models;
using EmployeeTrainingRegistrationServices.Entities;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTrainingRegistration.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;
        // GET: Register

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAccount(UserDetails user, UserAccount acc, Department dept)
        {
            if (_registerService.CheckRegister(user,acc, dept)) { return View("Success"); }
            else { return View("Error"); }
        }
    }
}