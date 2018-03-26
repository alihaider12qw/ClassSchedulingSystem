using ClassSchedulingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClassSchedulingSystem.Controllers
{
    public class CSSController : Controller
    {
        private CSS_DBEntities1 db = new CSS_DBEntities1();
        // GET: CSS
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index(int id, string pass)
        {
            var login = db.Students.Where(u => u.StudentID == id && u.Password == pass).ToList();
            if (login.Count == 1)
            {
                
                return RedirectToAction("Index", "Schedulers");
            }
            else
            {
                return View();

            }
        }
        public ActionResult Scheduler()
        {
            var schedules = db.Schedulers.ToList();
            return View(schedules);
        }
    }
}