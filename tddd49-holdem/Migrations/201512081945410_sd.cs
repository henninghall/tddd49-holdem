namespace tddd49_holdem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Table_TableId1", c => c.Int());
            CreateIndex("dbo.Players", "Table_TableId1");
            AddForeignKey("dbo.Players", "Table_TableId1", "dbo.Tables", "TableId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "Table_TableId1", "dbo.Tables");
            DropIndex("dbo.Players", new[] { "Table_TableId1" });
            DropColumn("dbo.Players", "Table_TableId1");
        }
    }
}
