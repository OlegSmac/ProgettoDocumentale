namespace ProgettoDocumentale.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeDocumentProjectNullable : DbMigration
    {
        public override void Up()
        {            
            DropForeignKey("dbo.Documents", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Documents", new[] { "ProjectId" });

            AlterColumn("dbo.Documents", "ProjectId", c => c.Int());

            CreateIndex("dbo.Documents", "ProjectId");
            AddForeignKey("dbo.Documents", "ProjectId", "dbo.Projects", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Documents", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Documents", new[] { "ProjectId" });

            AlterColumn("dbo.Documents", "ProjectId", c => c.Int(nullable: false));

            CreateIndex("dbo.Documents", "ProjectId");
            AddForeignKey("dbo.Documents", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
