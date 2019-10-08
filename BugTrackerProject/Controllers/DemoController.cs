using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTrackerProject.Models;
using System.Linq.Dynamic;

namespace BugTrackerProject.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult ShowGrid()
        {
            var ticket = db.Tickets.ToList();
            return View(ticket);
        }
        // [HttpPost]
      




    }


}