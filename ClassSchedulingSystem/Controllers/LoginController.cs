using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassSchedulingSystem.Models;
using System.Web.Security;

namespace ClassSchedulingSystem.Controllers
{
    public class LoginController : Controller
    {
        private CSS_DBEntities db = new CSS_DBEntities();
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Index(int? id,string pass)
        {
            if (id != null)
            {
                string title;
                var jn = from st in db.Students
                         join
                         tr in db.Teachers on st.CourseId equals tr.CourseID
                         select new { stID = st.StudentID, stP = st.Password, trID = tr.TeacherID, trP = tr.Password };
               
                foreach (var item in jn.ToList())
                {
                    if (item.stID == id )
                    {
                        if (item.stP.Equals(pass))
                        {
                            title = "Student";
                            return RedirectToAction("Index", "StudentSchedulers");
                        }
                    }
                    if(item.trID == id)
                    {
                        if (item.trP.Equals(pass))
                        {
                            title = "Teacher";
                            return RedirectToAction("Index", "Schedulers");
                        }
                    }
                }
                Response.Write("Wrong User ID or password");
                return View();
            }
            return View();

        }
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

    }
}