using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerProject.Models
{
    public class TicketManagement
    {
        ApplicationDbContext db;
        public TicketManagement(ApplicationDbContext db)
        {
            this.db = db;
        }
        public ApplicationUser CheckUserId(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = db.Users.Find(userId);
                if (user != null)
                {
                    return user;
                }
            }
            return null; // Or throw an exception;
        }
        public Ticket CheckTicketId(int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);
            if (ticket != null)
            {
                return ticket;
            }
            return null; // Or throw an exception;
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ICollection<Ticket> GetUserTickets(string userId)
        {
            var user = CheckUserId(userId);
            if (user != null)
            {
                return user.Tickets;
            }
            return null; // or throw and exceptions like HttpNotFound
        }
        //public ICollection<Ticket> GetTicketUsers(int ticketId)
        //{
        //    var ticket = CheckTicketId(ticketId);
        //    if (ticket != null)
        //    {
        //        return ticket.Tickets;
        //    }
        //    return null;
        //}
        /// <summary>
        /// Return all projects without any assigned users
        /// </summary>
        /// <returns></returns>
       
    }
}