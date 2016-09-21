namespace WebFramework.Persistence.Migrations.Products
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductsInitializeSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ColorCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dimensions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Length = c.Int(),
                        Width = c.Int(),
                        Depth = c.Int(),
                        Height = c.Int(),
                        Radius = c.Int(),
                        Diameter = c.Int(),
                        SizeUnit_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SizeUnits", t => t.SizeUnit_Id)
                .Index(t => t.SizeUnit_Id);
            
            CreateTable(
                "dbo.SizeUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        CurrentPrice = c.Decimal(precision: 18, scale: 2),
                        NewPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(),
                        UpdatedOn = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                        ImageUrl = c.String(),
                        BuyUrl = c.String(),
                        Category_Id = c.Int(),
                        Color_Id = c.Int(),
                        Dimension_Id = c.Int(),
                        Size_Id = c.Int(),
                        SubCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.Colors", t => t.Color_Id)
                .ForeignKey("dbo.Dimensions", t => t.Dimension_Id)
                .ForeignKey("dbo.Sizes", t => t.Size_Id)
                .ForeignKey("dbo.SubCategories", t => t.SubCategory_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.Color_Id)
                .Index(t => t.Dimension_Id)
                .Index(t => t.Size_Id)
                .Index(t => t.SubCategory_Id);
            
            CreateTable(
                "dbo.Sizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "SubCategory_Id", "dbo.SubCategories");
            DropForeignKey("dbo.Items", "Size_Id", "dbo.Sizes");
            DropForeignKey("dbo.Items", "Dimension_Id", "dbo.Dimensions");
            DropForeignKey("dbo.Items", "Color_Id", "dbo.Colors");
            DropForeignKey("dbo.Items", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Dimensions", "SizeUnit_Id", "dbo.SizeUnits");
            DropForeignKey("dbo.SubCategories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Items", new[] { "SubCategory_Id" });
            DropIndex("dbo.Items", new[] { "Size_Id" });
            DropIndex("dbo.Items", new[] { "Dimension_Id" });
            DropIndex("dbo.Items", new[] { "Color_Id" });
            DropIndex("dbo.Items", new[] { "Category_Id" });
            DropIndex("dbo.Dimensions", new[] { "SizeUnit_Id" });
            DropIndex("dbo.SubCategories", new[] { "Category_Id" });
            DropTable("dbo.Sizes");
            DropTable("dbo.Items");
            DropTable("dbo.SizeUnits");
            DropTable("dbo.Dimensions");
            DropTable("dbo.Colors");
            DropTable("dbo.SubCategories");
            DropTable("dbo.Categories");
        }
    }
}
