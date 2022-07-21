namespace HospitalCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelsadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        HasPic = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                        Description = c.String(),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Date = c.DateTime(nullable: false),
                        HasPic = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.FAQs",
                c => new
                    {
                        FAQId = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FAQId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Donors",
                c => new
                    {
                        DonorId = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        DepartmentId = c.Int(nullable: false),
                        Phone = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.DonorId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donors", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.FAQs", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Articles", "EventId", "dbo.Events");
            DropIndex("dbo.Donors", new[] { "DepartmentId" });
            DropIndex("dbo.FAQs", new[] { "DepartmentId" });
            DropIndex("dbo.Articles", new[] { "EventId" });
            DropTable("dbo.Donors");
            DropTable("dbo.FAQs");
            DropTable("dbo.Departments");
            DropTable("dbo.Events");
            DropTable("dbo.Articles");
        }
    }
}
