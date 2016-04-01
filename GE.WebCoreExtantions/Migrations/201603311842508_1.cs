namespace GE.WebCoreExtantions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.D_SEO_INFO", "MaterialId", c => c.Int());
            AddColumn("dbo.D_SEO_INFO", "ModelCoreType", c => c.Byte());
            CreateIndex("dbo.D_SEO_INFO", new[] { "MaterialId", "ModelCoreType" });
            AddForeignKey("dbo.D_SEO_INFO", new[] { "MaterialId", "ModelCoreType" }, "dbo.DV_MATERIAL", new[] { "Id", "ModelCoreType" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.D_SEO_INFO", new[] { "MaterialId", "ModelCoreType" }, "dbo.DV_MATERIAL");
            DropIndex("dbo.D_SEO_INFO", new[] { "MaterialId", "ModelCoreType" });
            DropColumn("dbo.D_SEO_INFO", "ModelCoreType");
            DropColumn("dbo.D_SEO_INFO", "MaterialId");
        }
    }
}
