namespace Passion_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trailsfeatures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrailFeatures",
                c => new
                    {
                        Trail_TrailID = c.Int(nullable: false),
                        Feature_FeatureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trail_TrailID, t.Feature_FeatureID })
                .ForeignKey("dbo.Trails", t => t.Trail_TrailID, cascadeDelete: true)
                .ForeignKey("dbo.Features", t => t.Feature_FeatureID, cascadeDelete: true)
                .Index(t => t.Trail_TrailID)
                .Index(t => t.Feature_FeatureID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrailFeatures", "Feature_FeatureID", "dbo.Features");
            DropForeignKey("dbo.TrailFeatures", "Trail_TrailID", "dbo.Trails");
            DropIndex("dbo.TrailFeatures", new[] { "Feature_FeatureID" });
            DropIndex("dbo.TrailFeatures", new[] { "Trail_TrailID" });
            DropTable("dbo.TrailFeatures");
        }
    }
}
