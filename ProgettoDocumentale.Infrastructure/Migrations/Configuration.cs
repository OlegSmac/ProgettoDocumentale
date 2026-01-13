namespace ProgettoDocumentale.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ProgettoDocumentale.Domain.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ProgettoDocumentale.Infrastructure.Persistence.ProgettoDocContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(ProgettoDocumentale.Infrastructure.Persistence.ProgettoDocContext context)
        {
            context.Roles.AddOrUpdate(
                r => r.Name,
                new Role { Name = "Admin" },
                new Role { Name = "OperatorCedacri" },
                new Role { Name = "OperatorBanc" }
            );
            context.SaveChanges();

            context.Institutions.AddOrUpdate(
                i => i.InstCode,
                new Institution
                {
                    InstCode = "SYS",
                    Name = "System Institution",
                    Created = DateTime.UtcNow,
                    CreatedBy = 0
                }
            );
            context.SaveChanges();

            var institution = context.Institutions.Single(i => i.InstCode == "SYS");

            var adminUser = context.Users.SingleOrDefault(u => u.UserName == "admin");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@system.local",
                    IsEnabled = true,
                    InstitutionId = institution.Id,
                    PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918"
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
            }

            var adminRole = context.Roles.Single(r => r.Name == "Admin");
            if (!context.UserToRoles.Any(x => x.UserId == adminUser.Id && x.RoleId == adminRole.Id))
            {
                context.UserToRoles.Add(new UserToRole
                {
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id
                });

                context.SaveChanges();
            }
        }
    }
}
