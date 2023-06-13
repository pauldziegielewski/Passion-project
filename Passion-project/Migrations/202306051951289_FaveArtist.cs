namespace Passion_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FaveArtist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FaveArtist", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FaveArtist");
        }
    }
}
