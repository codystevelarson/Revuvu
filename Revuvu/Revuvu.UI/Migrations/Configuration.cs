namespace Revuvu.UI.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Revuvu.UI.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Revuvu.UI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Revuvu.UI.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleMgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleMgr.RoleExists("Admin"))
            {
                roleMgr.Create(new IdentityRole() { Name = "Admin" });
            }

            if (!roleMgr.RoleExists("Marketing"))
            {
                roleMgr.Create(new IdentityRole() { Name = "Marketing" });
            }

            if (!roleMgr.RoleExists("User"))
            {
                roleMgr.Create(new IdentityRole() { Name = "User" });
            }            

            if (!roleMgr.RoleExists("Disabled"))
            {
                roleMgr.Create(new IdentityRole() { Name = "Disabled" });
            }


            if (!context.Users.Any(u => u.UserName == "RevuvuAdmin"))
            {
                var user = new ApplicationUser()
                {
                    UserName = "RevuvuAdmin",
                    Email = "Admin@Revuvu.com",
                };

                userMgr.Create(user, "123456");

                userMgr.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "RevuvuMarketing"))
            {
                var user = new ApplicationUser()
                {
                    UserName = "RevuvuMarketing",
                    Email = "Marketing@Revuvu.com",
                };

                userMgr.Create(user, "123456");

                userMgr.AddToRole(user.Id, "Marketing");
            }

            if (!context.Users.Any(u => u.UserName == "User1"))
            {
                var user = new ApplicationUser()
                {
                    UserName = "User1",
                    Email = "User1@Revuvu.com",
                };

                userMgr.Create(user, "123456");

                userMgr.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "User2"))
            {
                var user = new ApplicationUser()
                {
                    UserName = "User2",
                    Email = "User2@Revuvu.com",
                };

                userMgr.Create(user, "123456");

                userMgr.AddToRole(user.Id, "User");
            }


            if (!context.Users.Any(u => u.UserName == "BadUser"))
            {
                var user = new ApplicationUser()
                {
                    UserName = "BadUser",
                    Email = "BadUser@Revuvu.com",
                };

                userMgr.Create(user, "123456");

                userMgr.AddToRole(user.Id, "Disabled");
            }
        }
    }
}
