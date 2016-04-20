namespace GE.WebCoreExtantions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.D_MATERIAL_CATEGORY", "FrontPictureId", c => c.Guid());
            CreateIndex("dbo.D_MATERIAL_CATEGORY", "FrontPictureId");
            AddForeignKey("dbo.D_MATERIAL_CATEGORY", "FrontPictureId", "dbo.D_PICTURE", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.D_MATERIAL_CATEGORY", "FrontPictureId", "dbo.D_PICTURE");
            DropIndex("dbo.D_MATERIAL_CATEGORY", new[] { "FrontPictureId" });
            DropColumn("dbo.D_MATERIAL_CATEGORY", "FrontPictureId");
        }
    }
}
