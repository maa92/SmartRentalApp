namespace SmartRentalApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SmartRentalApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SmartRentalApp.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string[] appRoles = { "Admin", "Agent", "Customer" };

            foreach (string role in appRoles)
                if (!roleManager.RoleExists(role))
                    roleManager.Create(new IdentityRole(role));

            base.Seed(context);
        }
    }
}
