using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClassSchedulingSystem.Models;

namespace ClassSchedulingSystem.Controllers
{
    public class SchedulersController : Controller
    {
        private CSS_DBEntities db = new CSS_DBEntities();
        TimeSpan[] ts = {
            new TimeSpan(08, 30, 00),
            new TimeSpan(11, 30, 00),
            new TimeSpan(11, 45, 00),
            new TimeSpan(02, 45, 00),
            new TimeSpan(03, 00, 00),
            new TimeSpan(06, 00, 00),
            new TimeSpan(06, 15, 00),
            new TimeSpan(09, 15, 00)
        };
        // GET: Schedulers
        public ActionResult Index()
        {
            var schedulers = db.Schedulers.Include(s => s.Course).Include(s => s.Teacher).Include(s => s.Room);
            return View(schedulers.ToList());
        }

        // GET: Schedulers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scheduler scheduler = db.Schedulers.Find(id);
            if (scheduler == null)
            {
                return HttpNotFound();
            }
            return View(scheduler);
        }

        // GET: Schedulers/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name");
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "Name");
            return View();
        }

        // POST: Schedulers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int TeacherID, int CourseID)
        {
            //[Bind(Include = "RoomID,TeacherID,CourseID,DateTime,StartTime,EndTime")]
            Scheduler scheduler = new Scheduler();
            scheduler.RoomID = 102;
            scheduler.TeacherID = TeacherID;
            scheduler.CourseID = CourseID;
            scheduler.DayWeek = "Monday";
            scheduler.StartTime = ts[0];
            scheduler.EndTime = ts[1];

            List<Scheduler> clashes = ClashesChecker(scheduler);

            if (clashes.Count == 0) //No clash
            {
                if (ModelState.IsValid)
                {
                    db.Schedulers.Add(scheduler);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else    //Clashed
            {
                //T,DW->R
                string[] weekdays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Saturday" };

                while (ClashesChecker(scheduler).Count != 0)
                {
                    foreach (var r in clashes)
                    {
                        if (r.RoomID == scheduler.RoomID)
                        {
                            scheduler.RoomID += 1;
                        }
                        else if (r.RoomID + 1 != 106 && scheduler.RoomID != 106)    //if all rooms are not blocked
                        {
                            while (ClashesChecker(scheduler).Count != 0 && scheduler.RoomID != 106)
                            {
                                scheduler.RoomID += 1;
                            }
                        }
                        else if (r.StartTime == scheduler.StartTime)
                        {
                            scheduler.StartTime += (new TimeSpan(03, 15, 00));
                            scheduler.EndTime += (new TimeSpan(03, 15, 00));
                        }
                        else if (r.StartTime.ToString() != "15:00:00" && (scheduler.StartTime.ToString() != "15:00:00"))  //if rooms are blocked shift the time
                        {
                            while (ClashesChecker(scheduler).Count != 0 && scheduler.StartTime.ToString() != "15:00:00")
                            {
                                scheduler.StartTime += (new TimeSpan(03, 15, 00));
                                scheduler.EndTime += (new TimeSpan(03, 15, 00));
                            }
                        }
                        else if (r.DayWeek == scheduler.DayWeek)
                        {
                            for (int i2 = 0; i2 < 6; i2++)
                                if (r.DayWeek == weekdays[i2])
                                {
                                    scheduler.DayWeek = weekdays[i2 + 1];
                                    scheduler.RoomID = 102;
                                    scheduler.StartTime = new TimeSpan(08, 30, 00);
                                    scheduler.EndTime = new TimeSpan(11, 30, 00);
                                }
                        }
                        else if (scheduler.DayWeek != weekdays[5] && r.DayWeek != weekdays[5])    //if all rooms and time of same day is full, shift the day
                        {
                            while (ClashesChecker(scheduler).Count != 0 && scheduler.DayWeek != weekdays[5])
                            {
                                for (int i2 = 0; i2 < 6; i2++)
                                    if (r.DayWeek == weekdays[i2])
                                    {
                                        scheduler.DayWeek = weekdays[i2 + 1];
                                        scheduler.RoomID = 102;
                                        scheduler.StartTime = new TimeSpan(08, 30, 00);
                                        scheduler.EndTime = new TimeSpan(11, 30, 00);
                                    }
                            }
                        }
                        else    //if all rooms and time and weeks are utilized
                        {
                            Response.Write("Sorry you can not add any more schedule to the week");
                        }

                        if (ClashesChecker(scheduler).Count == 0)
                        {
                            if (ModelState.IsValid)
                            {
                                db.Schedulers.Add(scheduler);
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                            break;
                        }
                    }
                }
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name", scheduler.CourseID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "Name", scheduler.TeacherID);
            return View(scheduler);
        }

        public List<Scheduler> ClashesChecker(Scheduler scheduler)
        {
            var clashes = db.Schedulers.Where(scheduled => scheduler.DayWeek == scheduled.DayWeek && scheduler.RoomID == scheduled.RoomID && scheduled.StartTime == scheduler.StartTime).ToList();   //Clash checking
            return clashes;
        }

        // GET: Schedulers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scheduler scheduler = db.Schedulers.Find(id);
            if (scheduler == null)
            {
                return HttpNotFound();
            }
            return View(scheduler);
        }

        // POST: Schedulers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Scheduler scheduler = db.Schedulers.Find(id);
            db.Schedulers.Remove(scheduler);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
