using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrackerProject.Models;
using Microsoft.AspNet.Identity;

namespace BugTrackerProject.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       
        private ProjectManagement projectManager;
        //[Authorize(Roles = "Admin, Project Manager, Developer, Submitter")]
        public ProjectsController()
        {
            projectManager = new ProjectManagement(db);
        }
        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //[Authorize(Roles = "Admin, Project Manager")]
        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DateCreated")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DateCreated")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
        public ActionResult AssignProjects()
        {
            ViewBag.userId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.projectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AssignProjects(string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.ProjectUsers.Find(projectId);
            if (user.ProjectUsers == null)
                user.ProjectUsers = new List<ProjectUser>();
            user.ProjectUsers.Add(project);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult UnAssignProjects()
        {
            ViewBag.userId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.projectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult UnAssignProjects(string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.ProjectUsers.Find(projectId);
            if (user.ProjectUsers == null || user.ProjectUsers != null)
                user.ProjectUsers = new List<ProjectUser>();
            user.ProjectUsers.Remove(project);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        //public ActionResult ListAllProjectsByUser(string userProject)
        //{
        //    string input = HttpUtility.HtmlEncode(userProject);
        //    var projectsForUser = db.ProjectUsers.Select(s => s.ApplicationUser.Email).Distinct().ToList();
        //    ViewBag.listOfProjects = new SelectList(projectsForUser);
        //    var filtedProjects = db.ProjectUsers.Where(s => s.ApplicationUser.Email == input).ToList();
        //    return View(filtedProjects);
        //}

        public ActionResult ListAllProjectsByUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var allProjectsForUser = projectManager.GetUserProjects(userId).ToList();
                return View("Index", allProjectsForUser);
            }
            ViewBag.Message = "Please Log in to view this page";
            return View();

        }
    }
}
