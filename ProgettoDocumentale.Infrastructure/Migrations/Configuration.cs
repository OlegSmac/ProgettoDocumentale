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
            try
            {
                context.Roles.AddOrUpdate(
                r => r.Name,
                new Role { Name = "Admin" },
                new Role { Name = "CedacriOperator" },
                new Role { Name = "BancOperator" }
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
                        PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                        Name = "admin",
                        Surname = "admin",
                        Patronymic = "admin"
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

                var now = DateTime.UtcNow;
                const int systemUserId = 0;

                context.DocumentTypes.AddOrUpdate(
                    x => x.Code,
                    new DocumentType { Code = "SERV_REPORT", Name = "Report di servizio", TypeDscr = "Service reports", IsMarco = true, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },
                    new DocumentType { Code = "SLA_REPORT", Name = "Report SLA", TypeDscr = "SLA reports", IsMarco = true, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },
                    new DocumentType { Code = "PROGETTAZIONE", Name = "Progettazione", TypeDscr = "Project docs", IsMarco = true, IsDateGrouped = true, Created = now, CreatedBy = systemUserId }
                );
                context.SaveChanges();

                var servMacro = context.DocumentTypes.Single(x => x.Code == "SERV_REPORT");
                var slaMacro = context.DocumentTypes.Single(x => x.Code == "SLA_REPORT");
                var prjMacro = context.DocumentTypes.Single(x => x.Code == "PROGETTAZIONE");

                context.DocumentTypes.AddOrUpdate(
                    x => x.Code,

                    new DocumentType { Code = "SRV_NETWORK", Name = "Network", TypeDscr = "Network report", IsMarco = false, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },
                    new DocumentType { Code = "SRV_SECURITY", Name = "Sicurezza", TypeDscr = "Security report", IsMarco = false, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },
                    new DocumentType {Code = "SRV_CHANGE", Name = "Change", TypeDscr = "Change report", IsMarco = false, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },
                    new DocumentType {Code = "SRV_BACKUP", Name = "Backup", TypeDscr = "Backup report", IsMarco = false, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },

                    new DocumentType {Code = "PRJ_ANALISI", Name = "Analisi", TypeDscr = "Analysis", IsMarco = false, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },
                    new DocumentType {Code = "PRJ_TRANSIZIONE", Name = "Transizione", TypeDscr = "Transition", IsMarco = false, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },
                    new DocumentType {Code = "PRJ_PRODUZIONE", Name = "Produzione", TypeDscr = "Production", IsMarco = false, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },
                    new DocumentType {Code = "PRJ_TEST", Name = "Test", TypeDscr = "Testing", IsMarco = false, IsDateGrouped = true, Created = now, CreatedBy = systemUserId },
                    new DocumentType {Code = "PRJ_MONITORAGGIO", Name = "Monitoraggio", TypeDscr = "Monitoring", IsMarco = false, IsDateGrouped = true, Created = now, CreatedBy = systemUserId }
                );
                context.SaveChanges();

                var srvNetwork = context.DocumentTypes.Single(x => x.Code == "SRV_NETWORK");
                var srvSecurity = context.DocumentTypes.Single(x => x.Code == "SRV_SECURITY");
                var srvChange = context.DocumentTypes.Single(x => x.Code == "SRV_CHANGE");
                var srvBackup = context.DocumentTypes.Single(x => x.Code == "SRV_BACKUP");

                var prjAnalisi = context.DocumentTypes.Single(x => x.Code == "PRJ_ANALISI");
                var prjTransizione = context.DocumentTypes.Single(x => x.Code == "PRJ_TRANSIZIONE");
                var prjProduzione = context.DocumentTypes.Single(x => x.Code == "PRJ_PRODUZIONE");
                var prjTest = context.DocumentTypes.Single(x => x.Code == "PRJ_TEST");
                var prjMonitoraggio = context.DocumentTypes.Single(x => x.Code == "PRJ_MONITORAGGIO");

                context.DocumentTypeHierarchies.AddOrUpdate(
                    x => new { x.MacroId, x.MicroId },

                    new DocumentTypeHierarchy { MacroId = servMacro.Id, MicroId = srvNetwork.Id, Created = now, CreatedBy = systemUserId },
                    new DocumentTypeHierarchy { MacroId = servMacro.Id, MicroId = srvSecurity.Id, Created = now, CreatedBy = systemUserId },
                    new DocumentTypeHierarchy { MacroId = servMacro.Id, MicroId = srvChange.Id, Created = now, CreatedBy = systemUserId },
                    new DocumentTypeHierarchy { MacroId = servMacro.Id, MicroId = srvBackup.Id, Created = now, CreatedBy = systemUserId },

                    new DocumentTypeHierarchy { MacroId = prjMacro.Id, MicroId = prjAnalisi.Id, Created = now, CreatedBy = systemUserId },
                    new DocumentTypeHierarchy { MacroId = prjMacro.Id, MicroId = prjTransizione.Id, Created = now, CreatedBy = systemUserId },
                    new DocumentTypeHierarchy { MacroId = prjMacro.Id, MicroId = prjProduzione.Id, Created = now, CreatedBy = systemUserId },
                    new DocumentTypeHierarchy { MacroId = prjMacro.Id, MicroId = prjTest.Id, Created = now, CreatedBy = systemUserId },
                    new DocumentTypeHierarchy { MacroId = prjMacro.Id, MicroId = prjMonitoraggio.Id, Created = now, CreatedBy = systemUserId }
                );
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var inner = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                throw new Exception("Seed DbUpdateException: " + inner, ex);
            }
        }
    }
}
