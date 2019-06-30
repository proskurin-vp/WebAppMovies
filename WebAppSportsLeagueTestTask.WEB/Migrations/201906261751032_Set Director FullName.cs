namespace WebAppSportsLeagueTestTask.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetDirectorFullName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Directors", "FullName", c => c.String());
            DropColumn("dbo.Directors", "FirstName");
            DropColumn("dbo.Directors", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Directors", "LastName", c => c.String());
            AddColumn("dbo.Directors", "FirstName", c => c.String());
            DropColumn("dbo.Directors", "FullName");
        }
    }
}
