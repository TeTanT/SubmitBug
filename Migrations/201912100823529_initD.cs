namespace SubmitBug.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initD : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TB_Follow", "DId");
            AddForeignKey("dbo.TB_Follow", "DId", "dbo.TB_Difficultity", "DId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_Follow", "DId", "dbo.TB_Difficultity");
            DropIndex("dbo.TB_Follow", new[] { "DId" });
        }
    }
}
