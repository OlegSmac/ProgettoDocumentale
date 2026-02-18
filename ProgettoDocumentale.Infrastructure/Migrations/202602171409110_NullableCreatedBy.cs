namespace ProgettoDocumentale.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableCreatedBy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "CreatedBy", c => c.Int());
            AlterColumn("dbo.Documents", "Created", c => c.DateTime());
            AlterColumn("dbo.Institutions", "CreatedBy", c => c.Int());
            AlterColumn("dbo.Institutions", "Created", c => c.DateTime());
            AlterColumn("dbo.Projects", "CreatedBy", c => c.Int());
            AlterColumn("dbo.Projects", "Created", c => c.DateTime());
            AlterColumn("dbo.DocumentTypes", "CreatedBy", c => c.Int());
            AlterColumn("dbo.DocumentTypes", "Created", c => c.DateTime());
            AlterColumn("dbo.DocumentTypeHierarchies", "CreatedBy", c => c.Int());
            AlterColumn("dbo.DocumentTypeHierarchies", "Created", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DocumentTypeHierarchies", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DocumentTypeHierarchies", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.DocumentTypes", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DocumentTypes", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Projects", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Institutions", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Institutions", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Documents", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Documents", "CreatedBy", c => c.Int(nullable: false));
        }
    }
}
