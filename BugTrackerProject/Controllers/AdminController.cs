using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTrackerProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTrackerProject.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class AdminsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        RoleManager<IdentityRole> rolesManager;
        UserManager<ApplicationUser> usersManager;
        RoleManagement manager;
        // GET: Admins
        public AdminsController()
        {
            rolesManager = new RoleManager<IdentityRole>
               (new RoleStore<IdentityRole>(db));
            usersManager = new UserManager<ApplicationUser>
            (new UserStore<ApplicationUser>(db));
            manager = new RoleManagement(db);
        }
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult AssignRoles()
        {
            ViewBag.userId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.role = new SelectList(db.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult AssignRoles(string userId, string role)
        {
            usersManager.AddToRole(userId.ToString(), role);
            return RedirectToAction("Index");
        }
        public ActionResult UnAssignRoles()
        {
            ViewBag.userId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.role = new SelectList(db.Roles, "Name", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult UnAssignRoles(string userId, string role)
        {
            usersManager.RemoveFromRole(userId, role);
            return RedirectToAction("Index");
        }
        public ActionResult DisplayAllAdmins()
        {
            var allAdmins = manager.UsersInRole("Admin");
            return View(allAdmins);
        }
    }

}