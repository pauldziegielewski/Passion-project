namespace Passion_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trailPic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trails", "TrailHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Trails", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trails", "PicExtension");
            DropColumn("dbo.Trails", "TrailHasPic");
        }
    }
}
