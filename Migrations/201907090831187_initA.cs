namespace SubmitBug.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_BugSubmit",
                c => new
                    {
                        BId = c.Int(nullable: false, identity: true),
                        AppName = c.String(nullable: false, maxLength: 50),
                        Describe = c.String(nullable: false, maxLength: 500),
                        Claimant = c.String(nullable: false, maxLength: 50),
                        LPhone = c.String(nullable: false, maxLength: 50),
                        ComputerNo = c.String(nullable: false, maxLength: 20),
                        SubDate = c.DateTime(nullable: false),
                        LIP = c.String(maxLength: 50),
                        YN = c.String(maxLength: 2),
                        LId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BId)
                .ForeignKey("dbo.TB_LoginOn", t => t.LId, cascadeDelete: true)
                .Index(t => t.LId);
            
            CreateTable(
                "dbo.TB_Follow",
                c => new
                    {
                        FId = c.Int(nullable: false, identity: true),
                        Schedule = c.String(nullable: false, maxLength: 100),
                        FollowDate = c.DateTime(),
                        LId = c.Int(nullable: false),
                        BId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FId)
                .ForeignKey("dbo.TB_BugSubmit", t => t.BId, cascadeDelete: true)
                .ForeignKey("dbo.TB_LoginOn", t => t.LId, cascadeDelete: false)
                .Index(t => t.LId)
                .Index(t => t.BId);
            
            CreateTable(
                "dbo.TB_LoginOn",
                c => new
                    {
                        LId = c.Int(nullable: false, identity: true),
                        LoginNo = c.String(nullable: false, maxLength: 20),
                        LoginName = c.String(nullable: false, maxLength: 50),
                        LoginPwd = c.String(nullable: false, maxLength: 50),
                        LPhone = c.String(nullable: false, maxLength: 50),
                        ComputerNo = c.String(nullable: false, maxLength: 20),
                        AId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LId)
                .ForeignKey("dbo.TB_UserAccess", t => t.AId, cascadeDelete: true)
                .Index(t => t.AId);
            
            CreateTable(
                "dbo.TB_UserAccess",
                c => new
                    {
                        AId = c.Int(nullable: false, identity: true),
                        AccessName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.AId);
            
            CreateTable(
                "dbo.TB_Schedule",
                c => new
                    {
                        SId = c.Int(nullable: false, identity: true),
                        FinishDate = c.DateTime(nullable: false),
                        BId = c.Int(nullable: false),
                        FId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SId)
                .ForeignKey("dbo.TB_BugSubmit", t => t.BId, cascadeDelete: true)
                .ForeignKey("dbo.TB_Follow", t => t.FId, cascadeDelete: false)
                .Index(t => t.BId)
                .Index(t => t.FId);
            
            CreateTable(
                "dbo.TB_Role",
                c => new
                    {
                        RId = c.Int(nullable: false, identity: true),
                        RoleEncoding = c.String(nullable: false, maxLength: 20),
                        RoleName = c.String(nullable: false, maxLength: 20),
                        RoleDesc = c.String(nullable: false, maxLength: 20),
                        AddRoleDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RId);
            
            CreateTable(
                "dbo.TB_RoleAction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleEncodings = c.String(nullable: false, maxLength: 500),
                        RId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TB_Role", t => t.RId, cascadeDelete: true)
                .Index(t => t.RId);
            
            CreateTable(
                "dbo.TB_Users",
                c => new
                    {
                        UId = c.Int(nullable: false, identity: true),
                        LoginNo = c.String(nullable: false, maxLength: 20),
                        LoginName = c.String(nullable: false, maxLength: 50),
                        LoginPwd = c.String(nullable: false, maxLength: 50),
                        LPhone = c.String(nullable: false, maxLength: 50),
                        ComputerNo = c.String(nullable: false, maxLength: 20),
                        RId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UId)
                .ForeignKey("dbo.TB_Role", t => t.RId, cascadeDelete: true)
                .Index(t => t.RId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_Users", "RId", "dbo.TB_Role");
            DropForeignKey("dbo.TB_RoleAction", "RId", "dbo.TB_Role");
            DropForeignKey("dbo.TB_Schedule", "FId", "dbo.TB_Follow");
            DropForeignKey("dbo.TB_Schedule", "BId", "dbo.TB_BugSubmit");
            DropForeignKey("dbo.TB_LoginOn", "AId", "dbo.TB_UserAccess");
            DropForeignKey("dbo.TB_Follow", "LId", "dbo.TB_LoginOn");
            DropForeignKey("dbo.TB_BugSubmit", "LId", "dbo.TB_LoginOn");
            DropForeignKey("dbo.TB_Follow", "BId", "dbo.TB_BugSubmit");
            DropIndex("dbo.TB_Users", new[] { "RId" });
            DropIndex("dbo.TB_RoleAction", new[] { "RId" });
            DropIndex("dbo.TB_Schedule", new[] { "FId" });
            DropIndex("dbo.TB_Schedule", new[] { "BId" });
            DropIndex("dbo.TB_LoginOn", new[] { "AId" });
            DropIndex("dbo.TB_Follow", new[] { "BId" });
            DropIndex("dbo.TB_Follow", new[] { "LId" });
            DropIndex("dbo.TB_BugSubmit", new[] { "LId" });
            DropTable("dbo.TB_Users");
            DropTable("dbo.TB_RoleAction");
            DropTable("dbo.TB_Role");
            DropTable("dbo.TB_Schedule");
            DropTable("dbo.TB_UserAccess");
            DropTable("dbo.TB_LoginOn");
            DropTable("dbo.TB_Follow");
            DropTable("dbo.TB_BugSubmit");
        }
    }
}
