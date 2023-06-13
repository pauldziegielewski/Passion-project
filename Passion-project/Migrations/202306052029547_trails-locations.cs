namespace Passion_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trailslocations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trails", "LocationID", c => c.Int(nullable: false));
            CreateIndex("dbo.Trails", "LocationID");
            AddForeignKey("dbo.Trails", "LocationID", "dbo.Locations", "LocationID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trails", "LocationID", "dbo.Locations");
            DropIndex("dbo.Trails", new[] { "LocationID" });
            DropColumn("dbo.Trails", "LocationID");
        }
    }
}
