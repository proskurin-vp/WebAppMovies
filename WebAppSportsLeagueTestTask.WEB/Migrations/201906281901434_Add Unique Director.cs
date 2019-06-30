namespace WebAppSportsLeagueTestTask.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueDirector : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Directors", "FullName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Movies", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Description", c => c.String(nullable: false));
            CreateIndex("dbo.Directors", "FullName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Directors", new[] { "FullName" });
            AlterColumn("dbo.Movies", "Description", c => c.String());
            AlterColumn("dbo.Movies", "Name", c => c.String());
            AlterColumn("dbo.Directors", "FullName", c => c.String());
        }
    }
}
