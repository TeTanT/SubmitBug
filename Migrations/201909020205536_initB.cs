namespace SubmitBug.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_ShowMenu",
                c => new
                    {
                        MId = c.Int(nullable: false, identity: true),
                        MenuName = c.String(nullable: false, maxLength: 20),
                        AId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MId)
                .ForeignKey("dbo.TB_UserAccess", t => t.AId, cascadeDelete: true)
                .Index(t => t.AId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_ShowMenu", "AId", "dbo.TB_UserAccess");
            DropIndex("dbo.TB_ShowMenu", new[] { "AId" });
            DropTable("dbo.TB_ShowMenu");
        }
    }
}
