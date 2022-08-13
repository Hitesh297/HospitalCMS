namespace HospitalCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCommentModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentText = c.String(),
                        ArticleId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: true),
                        PatientId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.DoctorId)
                .Index(t => t.PatientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Comments", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Comments", "ArticleId", "dbo.Articles");
            DropIndex("dbo.Comments", new[] { "PatientId" });
            DropIndex("dbo.Comments", new[] { "DoctorId" });
            DropIndex("dbo.Comments", new[] { "ArticleId" });
            DropTable("dbo.Comments");
        }
    }
}
