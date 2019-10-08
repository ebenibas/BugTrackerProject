using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTrackerProject.Models
{
    public class RoleManagement
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> usersManager;
        public RoleManagement(ApplicationDbContext db)
        {
            this.db = db;
            usersManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        public bool IsUserInRole(string userId, string roleName)
        {
            return usersManager.IsInRole(userId, roleName);
        }
        public ICollection<string> GetUserRoles(string userId)
        {
            return usersManager.GetRoles(userId);
        }
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = usersManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = usersManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }
        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var result = new List<ApplicationUser>();
            var allUsers = db.Users.ToList();
            foreach (var user in allUsers)
            {
                //Another way to solve it with Linq inside the foreach
                //usersManager.GetRoles(user.Id).Any(r => r == roleName);
                if (IsUserInRole(user.Id, roleName))
                {
                    result.Add(user);
                }
            }
            //Another way with Linq witout foreach
            //return db.Users.Where(u => IsUserInRole(u.Id, roleName)).ToList();

            return result;
        }
        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var result = new List<ApplicationUser>();
            var allUsers = db.Users;
            foreach (var user in allUsers)
            {
                if (!IsUserInRole(user.Id, roleName))
                {
                    result.Add(user);
                }
            }
            return result;
        }
    }
    
}