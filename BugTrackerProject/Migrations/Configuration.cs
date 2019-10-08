namespace BugTrackerProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTrackerProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BugTrackerProject.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var RoleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            RoleManager.Create(new IdentityRole("Admin"));
            RoleManager.Create(new IdentityRole("ProjectManager"));
            RoleManager.Create(new IdentityRole("Developer"));
            RoleManager.Create(new IdentityRole("Submitter"));
        }
    }
}
