using DataAccessLayer.DBConnection;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.IRepositories;
using EmployeeTrainingRegistration.Models;
using EmployeeTrainingRegistrationServices.Interfaces;
using EmployeeTrainingRegistrationServices.Validation.IValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTrainingRegistration.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
    
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public ActionResult Login()
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
        public ActionResult Verify(UserAccount acc)
        {
             if (_loginService.CheckLogin(acc)) { return View("Success"); }
             else { return View("Error"); }
          
            
        }

    }
}