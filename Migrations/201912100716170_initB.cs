namespace SubmitBug.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_Difficultity",
                c => new
                    {
                        DId = c.Int(nullable: false, identity: true),
                        Level = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.DId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TB_Difficultity");
        }
    }
}
