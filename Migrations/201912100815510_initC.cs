namespace SubmitBug.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_Follow", "DId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TB_Follow", "DId");
        }
    }
}
