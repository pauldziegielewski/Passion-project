namespace Passion_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class locations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                    })
                .PrimaryKey(t => t.LocationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Locations");
        }
    }
}
