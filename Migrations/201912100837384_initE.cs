namespace SubmitBug.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_Difficultity", "DLevel", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.TB_Difficultity", "Level");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TB_Difficultity", "Level", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.TB_Difficultity", "DLevel");
        }
    }
}
