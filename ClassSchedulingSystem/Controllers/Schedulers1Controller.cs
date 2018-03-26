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
    public class Schedulers1Controller : Controller
    {
        private CSS_DBEntities1 db = new CSS_DBEntities1();

        // GET: Schedulers1
        public ActionResult Index()
        {
            var schedulers = db.Schedulers.Include(s => s.Course).Include(s => s.Room).Include(s => s.Teacher);
            return View(schedulers.ToList());
        }

        // GET: Schedulers1/Details/5
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

        // GET: Schedulers1/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name");
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "Seats");
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "Name");
            return View();
        }

        // POST: Schedulers1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoomID,TeacherID,CourseID,DateTime,StartTime,EndTime")] Scheduler scheduler)
        {
            if (ModelState.IsValid)
            {
                db.Schedulers.Add(scheduler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name", scheduler.CourseID);
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "Seats", scheduler.RoomID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "Name", scheduler.TeacherID);
            return View(scheduler);
        }

        // GET: Schedulers1/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name", scheduler.CourseID);
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "Seats", scheduler.RoomID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "Name", scheduler.TeacherID);
            return View(scheduler);
        }

        // POST: Schedulers1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoomID,TeacherID,CourseID,DateTime,StartTime,EndTime")] Scheduler scheduler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scheduler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Name", scheduler.CourseID);
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "Seats", scheduler.RoomID);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "Name", scheduler.TeacherID);
            return View(scheduler);
        }

        // GET: Schedulers1/Delete/5
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

        // POST: Schedulers1/Delete/5
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
