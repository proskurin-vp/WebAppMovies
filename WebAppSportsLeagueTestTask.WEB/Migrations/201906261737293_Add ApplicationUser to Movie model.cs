namespace WebAppSportsLeagueTestTask.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationUsertoMoviemodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Movies", "ApplicationUser_Id");
            AddForeignKey("dbo.Movies", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Movies", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Movies", "ApplicationUser_Id");
        }
    }
}
