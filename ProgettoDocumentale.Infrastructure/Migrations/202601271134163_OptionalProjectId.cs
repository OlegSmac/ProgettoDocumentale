namespace ProgettoDocumentale.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalProjectId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Documents", new[] { "ProjectId" });
            AlterColumn("dbo.Documents", "ProjectId", c => c.Int());
            CreateIndex("dbo.Documents", "ProjectId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Documents", new[] { "ProjectId" });
            AlterColumn("dbo.Documents", "ProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "ProjectId");
        }
    }
}
