namespace Passion_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trails",
                c => new
                    {
                        TrailID = c.Int(nullable: false, identity: true),
                        TrailName = c.String(),
                    })
                .PrimaryKey(t => t.TrailID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Trails");
        }
    }
}
