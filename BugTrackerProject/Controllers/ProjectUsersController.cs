using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrackerProject.Models;

namespace BugTrackerProject.Controllers
{
    public class ProjectUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectUsers
        public ActionResult Index()
        {
            var projectUsers = db.ProjectUsers.Include(p => p.ApplicationUser).Include(p => p.Project);
            return View(projectUsers.ToList());
        }

        // GET: ProjectUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUser projectUser = db.ProjectUsers.Find(id);
            if (projectUser == null)
            {
                return HttpNotFound();
            }
            return View(projectUser);
        }

        // GET: ProjectUsers/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        // POST: ProjectUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,ApplicationUserId")] ProjectUser projectUser)
        {
            if (ModelState.IsValid)
            {
                db.ProjectUsers.Add(projectUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", projectUser.ApplicationUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectUser.ProjectId);
            return View(projectUser);
        }

        // GET: ProjectUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUser projectUser = db.ProjectUsers.Find(id);
            if (projectUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", projectUser.ApplicationUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectUser.ProjectId);
            return View(projectUser);
        }

        // POST: ProjectUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,ApplicationUserId")] ProjectUser projectUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", projectUser.ApplicationUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectUser.ProjectId);
            return View(projectUser);
        }

        // GET: ProjectUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUser projectUser = db.ProjectUsers.Find(id);
            if (projectUser == null)
            {
                return HttpNotFound();
            }
            return View(projectUser);
        }

        // POST: ProjectUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectUser projectUser = db.ProjectUsers.Find(id);
            db.ProjectUsers.Remove(projectUser);
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
