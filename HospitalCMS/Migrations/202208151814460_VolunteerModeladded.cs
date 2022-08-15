namespace HospitalCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerModeladded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        VolunteerId = c.Int(nullable: false, identity: true),
                        VolunteerName = c.String(),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VolunteerId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Volunteers", "EventId", "dbo.Events");
            DropIndex("dbo.Volunteers", new[] { "EventId" });
            DropTable("dbo.Volunteers");
        }
    }
}
