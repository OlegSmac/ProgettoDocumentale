namespace ProgettoDocumentale.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstitutionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 260),
                        SavedPath = c.String(maxLength: 3000),
                        UploadDate = c.DateTime(nullable: false),
                        AdditionalInfo = c.String(maxLength: 1000),
                        GroupingDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastModifiedBy = c.Int(),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutions", t => t.InstitutionId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.DocumentTypes", t => t.TypeId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.InstitutionId)
                .Index(t => t.UserId)
                .Index(t => t.TypeId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Institutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstCode = c.String(nullable: false, maxLength: 5),
                        Name = c.String(nullable: false, maxLength: 255),
                        AdditionalInfo = c.String(maxLength: 1000),
                        CreatedBy = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastModifiedBy = c.Int(),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstitutionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        DateFrom = c.DateTime(nullable: false),
                        DateTill = c.DateTime(nullable: false),
                        AdditionalInfo = c.String(maxLength: 1000),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastModifiedBy = c.Int(),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutions", t => t.InstitutionId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.InstitutionId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstitutionId = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 32),
                        PasswordHash = c.String(),
                        Email = c.String(nullable: false, maxLength: 254),
                        IsEnabled = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 100),
                        Surname = c.String(maxLength: 100),
                        Patronymic = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutions", t => t.InstitutionId)
                .Index(t => t.InstitutionId);
            
            CreateTable(
                "dbo.UserToRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 5),
                        Name = c.String(maxLength: 255),
                        TypeDscr = c.String(maxLength: 500),
                        IsMarco = c.Boolean(nullable: false),
                        IsDateGrouped = c.Boolean(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastModifiedBy = c.Int(),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentTypeHierarchies",
                c => new
                    {
                        MacroId = c.Int(nullable: false),
                        MicroId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastModifiedBy = c.Int(),
                        LastModified = c.DateTime(),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MacroId, t.MicroId })
                .ForeignKey("dbo.DocumentTypes", t => t.MacroId)
                .ForeignKey("dbo.DocumentTypes", t => t.MicroId)
                .Index(t => t.MacroId)
                .Index(t => t.MicroId);

            CreateIndex("dbo.Documents", "InstitutionId", name: "IX_Documents_InstitutionId");
            CreateIndex("dbo.Documents", "UserId", name: "IX_Documents_UserId");
            CreateIndex("dbo.Documents", "TypeId", name: "IX_Documents_TypeId");
            CreateIndex("dbo.Documents", "ProjectId", name: "IX_Documents_ProjectId");

            CreateIndex("dbo.DocumentTypes", "Code", name: "IX_DocumentTypes_Code");
            CreateIndex("dbo.DocumentTypes", "Name", name: "IX_DocumentTypes_Name");

            CreateIndex("dbo.Institutions", "InstCode", unique: true, name: "IX_Institutions_InstCode");

            CreateIndex("dbo.Projects", "InstitutionId", name: "IX_Projects_InstitutionId");
            CreateIndex("dbo.Projects", "UserId", name: "IX_Projects_UserId");

            CreateIndex("dbo.Roles", "Name", unique: true, name: "IX_Roles_Name");

            CreateIndex("dbo.Users", "InstitutionId", name: "IX_Users_InstitutionId");
            CreateIndex("dbo.Users", "UserName", unique: true, name: "IX_Users_UserName");
            CreateIndex("dbo.Users", "Email", unique: true, name: "IX_Users_Email");

            CreateIndex("dbo.UserToRoles", "RoleId", name: "IX_UserToRoles_RoleId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "UserId", "dbo.Users");
            DropForeignKey("dbo.Documents", "TypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentTypeHierarchies", "MicroId", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentTypeHierarchies", "MacroId", "dbo.DocumentTypes");
            DropForeignKey("dbo.Documents", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Documents", "InstitutionId", "dbo.Institutions");
            DropForeignKey("dbo.Projects", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserToRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserToRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "InstitutionId", "dbo.Institutions");
            DropForeignKey("dbo.Projects", "InstitutionId", "dbo.Institutions");
            DropIndex("dbo.DocumentTypeHierarchies", new[] { "MicroId" });
            DropIndex("dbo.DocumentTypeHierarchies", new[] { "MacroId" });
            DropIndex("dbo.Roles", new[] { "Name" });
            DropIndex("dbo.UserToRoles", new[] { "RoleId" });
            DropIndex("dbo.UserToRoles", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "InstitutionId" });
            DropIndex("dbo.Projects", new[] { "UserId" });
            DropIndex("dbo.Projects", new[] { "InstitutionId" });
            DropIndex("dbo.Documents", new[] { "ProjectId" });
            DropIndex("dbo.Documents", new[] { "TypeId" });
            DropIndex("dbo.Documents", new[] { "UserId" });
            DropIndex("dbo.Documents", new[] { "InstitutionId" });
            DropTable("dbo.DocumentTypeHierarchies");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.Roles");
            DropTable("dbo.UserToRoles");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.Institutions");
            DropTable("dbo.Documents");

            DropIndex("dbo.UserToRoles", "IX_UserToRoles_RoleId");

            DropIndex("dbo.Users", "IX_Users_Email");
            DropIndex("dbo.Users", "IX_Users_UserName");
            DropIndex("dbo.Users", "IX_Users_InstitutionId");

            DropIndex("dbo.Roles", "IX_Roles_Name");

            DropIndex("dbo.Projects", "IX_Projects_UserId");
            DropIndex("dbo.Projects", "IX_Projects_InstitutionId");

            DropIndex("dbo.Institutions", "IX_Institutions_InstCode");

            DropIndex("dbo.DocumentTypes", "IX_DocumentTypes_Name");
            DropIndex("dbo.DocumentTypes", "IX_DocumentTypes_Code");

            DropIndex("dbo.Documents", "IX_Documents_ProjectId");
            DropIndex("dbo.Documents", "IX_Documents_TypeId");
            DropIndex("dbo.Documents", "IX_Documents_UserId");
            DropIndex("dbo.Documents", "IX_Documents_InstitutionId");
        }
    }
}
