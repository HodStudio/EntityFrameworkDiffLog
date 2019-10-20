namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLogEntries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(),
                        EntityName = c.String(),
                        EntityId = c.String(),
                        Operation = c.String(),
                        LogDateTime = c.DateTime(nullable: false),
                        ValuesChanges = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogEntries");
        }
    }
}
