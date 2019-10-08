using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerProject.Models
{
    public class ProjectManagement
    {

        ApplicationDbContext db;
        public ProjectManagement(ApplicationDbContext db)
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
        public Project CheckProjectId(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project != null)
            {
                return project;
            }
            return null; // Or throw an exception;
        }
        public bool AssignUserToProject(string userId, int projectId)
        {
            var user = CheckUserId(userId);
            var project = CheckProjectId(projectId);
            if (user != null && project != null)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UnAssignUserFromProject(string userId, int projectId)
        {
            var user = CheckUserId(userId);
            var project = CheckProjectId(projectId);
            if (user != null && project != null)
            {
                db.Projects.Remove(project);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ICollection<ProjectUser> GetUserProjects(string userId)
        {
            var user = CheckUserId(userId);
            if (user != null)
            {
                return user.ProjectUsers;
            }
            return null; // or throw and exceptions like HttpNotFound
        }
        public ICollection<ProjectUser> GetProjectUsers(int projectId)
        {
            var project = CheckProjectId(projectId);
            if (project != null)
            {
                return project.ProjectUsers;
            }
            return null;
        }
        /// <summary>
        /// Return all projects without any assigned users
        /// </summary>
        /// <returns></returns>
        public List<Project> GetNewProjects()
        {
            var allProjects = db.Projects.ToList();
            return allProjects.Where(p => p.ProjectUsers.Count() == 0).ToList();
        }
    }
}
