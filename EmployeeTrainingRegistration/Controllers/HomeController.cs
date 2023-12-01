
using DataAccessLayer.DBConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTrainingRegistration.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IDataAccessLayer _layer;

        public HomeController(IDataAccessLayer layer)
        {
            //_layer = layer;
        }
        public ActionResult Index()
        {
            /*ViewBag.Message = _layer.Connect();
            ViewBag.MyName = "MVC p trvy";
            return View();*/
            return RedirectToAction("Login","Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}