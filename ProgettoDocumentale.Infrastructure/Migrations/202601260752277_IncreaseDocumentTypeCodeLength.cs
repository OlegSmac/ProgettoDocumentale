namespace ProgettoDocumentale.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseDocumentTypeCodeLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DocumentTypes", "Code", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DocumentTypes", "Code", c => c.String(maxLength: 5));
        }
    }
}
