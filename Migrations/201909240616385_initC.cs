namespace SubmitBug.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_UserFace",
                c => new
                    {
                        UId = c.Int(nullable: false, identity: true),
                        UserFace = c.String(nullable: false, maxLength: 50),
                        LId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UId)
                .ForeignKey("dbo.TB_LoginOn", t => t.LId, cascadeDelete: true)
                .Index(t => t.LId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_UserFace", "LId", "dbo.TB_LoginOn");
            DropIndex("dbo.TB_UserFace", new[] { "LId" });
            DropTable("dbo.TB_UserFace");
        }
    }
}
