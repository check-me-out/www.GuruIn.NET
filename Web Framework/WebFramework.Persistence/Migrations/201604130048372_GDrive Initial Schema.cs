namespace WebFramework.Persistence.Migrations.Blog
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GDriveInitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SecurityCode = c.String(nullable: false, maxLength: 256),
                    FileName = c.String(nullable: false, maxLength: 256),
                    Content = c.Binary(nullable: true),
                    UploadedOn = c.DateTime(nullable: true),
                    Test = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.FileName, unique: true, name: "FileNameIndex")
                .Index(t => t.SecurityCode, unique: false, name: "SecurityCodeIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Files", new[] { "SecurityCode", "FileName" });
            DropTable("dbo.Files");
        }
    }
}
