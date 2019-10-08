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
using System.Linq.Dynamic;

namespace BugTrackerProject.Controllers
{
    public class TicketsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        private TicketManagement TicketManager;
        public TicketsController()
        {
            TicketManager = new TicketManagement(db);
        }
        //GET: Tickets
        //[Authorize(Roles = "Admin, Project Manager, Developer, Submitter")]
        public ActionResult Index()
        {

            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).
                Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);

            return View(tickets.ToList());

        }
        [HttpPost]
        public ActionResult GetList1()
        {
            var tickets = db.Tickets.ToList();
            return View(tickets);

        }
        [HttpPost]

        public ActionResult getData()
        {
            //Datatable parameter
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //paging parameter
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //sorting parameter
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            //filter parameter
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
            var allTickets = db.Tickets.ToList();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            //Database query
          
            
                var v = (from a in db.Tickets select a);
            //search
            if (!string.IsNullOrEmpty(searchValue))
            {
                v = v.Where(a =>
                    a.AssignedToUserId.Contains(searchValue) ||
                    a.OwnerUserId.Contains(searchValue) ||
                    a.Project.Name.Contains(searchValue) ||
                    a.TicketPriority.Name.Contains(searchValue) ||
                    a.TicketStatus.Name.Contains(searchValue)
                     ||
                    a.TicketType.Name.Contains(searchValue) ||
                    a.Title.Contains(searchValue) ||
                    a.Description.Contains(searchValue) ||
                    a.Created.ToString().Contains(searchValue)||
                    a.Updated.ToString().Contains(searchValue)
                        );
                }
                //sort
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    //for make sort simpler we will add Syste.Linq.Dynamic reference
                    v = v.OrderBy(sortColumn + " " + sortColumnDir);
                }
                recordsTotal = v.Count();
                allTickets = v.Skip(skip).Take(pageSize).ToList();
            
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allTickets });
        }




        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "Email", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "Email", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "Email", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
        public ActionResult ListAllTicketsByUser()
        {
            if (User.IsInRole("Submitter"))
            {
                var userId = User.Identity.GetUserId();
                var allTicketsForUser = TicketManager.GetUserTickets(userId).ToList();
                return View("Index", allTicketsForUser);
            }
            else if (User.IsInRole("Developer"))
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var assignedTickets = user.Tickets.ToList();
                return View(assignedTickets);
            }
            else if (User.IsInRole("projectManager"))
            {
                var projectId = User.Identity.GetUserId();
                var user = db.Users.Find(projectId);
                var assignedTicketToUser = user.Tickets.ToList();
                return View(assignedTicketToUser);
            }
            else if (User.IsInRole("Admin"))
            {
                return View(db.Tickets.ToList());
            }

            ViewBag.Message = "Please Log in to view this page";
           
            return View();
        }
    }
}
