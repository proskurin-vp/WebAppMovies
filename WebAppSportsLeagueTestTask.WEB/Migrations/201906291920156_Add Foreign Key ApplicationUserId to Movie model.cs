namespace WebAppSportsLeagueTestTask.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyApplicationUserIdtoMoviemodel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Movies", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Movies", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Movies", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Movies", name: "ApplicationUserId", newName: "ApplicationUser_Id");
        }
    }
}
