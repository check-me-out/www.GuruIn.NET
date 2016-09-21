namespace WebFramework.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using WebFramework.Persistence.Helpers;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        UrlSlug = c.String(nullable: true, maxLength: 512),
                        Description = c.String(nullable: true, maxLength: 1024),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "CategoryNameIndex");

            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 512),
                        ShortDescription = c.String(nullable: true, maxLength: 1024),
                        Content = c.String(nullable: false),
                        UrlSlug = c.String(nullable: true, maxLength: 512),
                        Published = c.Boolean(nullable: false, defaultValue: true),
                        PostedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: true),
                        Category_Id = c.Int(nullable: true)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id, name: "PostCategoryIndex");

            CreateTable(
                "dbo.Tags",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 256),
                    UrlSlug = c.String(nullable: true, maxLength: 512),
                    Description = c.String(nullable: true, maxLength: 1024),
                    Class = c.String(nullable: true, maxLength: 100),
                    Post_Id = c.Int(nullable: true),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Name, name: "TagNameIndex")
                .Index(t => t.Post_Id, name: "TagPostIndex");

            CreateTable(
                "dbo.Comments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 256),
                    Email = c.String(nullable: true, maxLength: 512),
                    Content = c.String(nullable: false, maxLength: 1024),
                    CommentedOn = c.DateTime(nullable: false),
                    NotifyOnComments = c.Boolean(nullable: false, defaultValue: false),
                    NotifyOnPosts = c.Boolean(nullable: false, defaultValue: false),
                    Post_Id = c.Int(nullable: true),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Post_Id, name: "CommentPostIndex");

            CreateTable(
                "dbo.BadWords",
                c => new
                {
                    Keyword = c.String(nullable: false, maxLength: 512),
                })
                .PrimaryKey(t => t.Keyword);

            //var script = EmbeddedResource.GetContents("WebFramework.Persistence.Migrations.v1_0.InstallScript.sql");
            //Sql(script);
        }

        public override void Down()
        {
            //var script = EmbeddedResource.GetContents("WebFramework.Persistence.Migrations.v1_0.UninstallScript.sql");
            //Sql(script);

            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Tags", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Posts", "Category_Id", "dbo.Categories");

            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropIndex("dbo.Tags", new[] { "Post_Id" });
            DropIndex("dbo.Tags", new[] { "Name" });
            DropIndex("dbo.Posts", new[] { "Category_Id" });
            DropIndex("dbo.Categories", new[] { "Name" });

            DropTable("dbo.BadWords");
            DropTable("dbo.Comments");
            DropTable("dbo.Tags");
            DropTable("dbo.Posts");
            DropTable("dbo.Categories");
        }
    }
}
