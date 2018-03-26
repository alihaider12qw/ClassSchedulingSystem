using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassSchedulingSystem.Models;

namespace ClassSchedulingSystem.Controllers
{
    public class StudentSchedulersController : Controller
    {
        private CSS_DBEntities db = new CSS_DBEntities();

        // GET: StudentSchedulers
        public ActionResult Index()
        {
            var schedulers = db.Schedulers.Include(s => s.Course).Include(s => s.Room).Include(s => s.Teacher);
            return View(schedulers.ToList());
        }

    }
}
