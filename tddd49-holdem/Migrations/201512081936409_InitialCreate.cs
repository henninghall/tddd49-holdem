namespace tddd49_holdem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        CardId = c.Int(nullable: false, identity: true),
                        Suit = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                        SuitSymbol = c.String(),
                        ValueSymbol = c.String(),
                        Color = c.String(),
                        Show = c.Boolean(nullable: false),
                        Player_Name = c.String(maxLength: 128),
                        Table_TableId = c.Int(),
                    })
                .PrimaryKey(t => t.CardId)
                .ForeignKey("dbo.Players", t => t.Player_Name)
                .ForeignKey("dbo.Tables", t => t.Table_TableId)
                .Index(t => t.Player_Name)
                .Index(t => t.Table_TableId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        ChipsOnHand = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CurrentBet = c.Int(nullable: false),
                        CanCheck = c.Boolean(nullable: false),
                        CanFold = c.Boolean(nullable: false),
                        CanCall = c.Boolean(nullable: false),
                        CanRaise = c.Boolean(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Table_TableId = c.Int(),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Tables", t => t.Table_TableId)
                .Index(t => t.Table_TableId);
            
            CreateTable(
                "dbo.Rows",
                c => new
                    {
                        RowId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Color = c.String(),
                        TimeStamp = c.String(),
                        LogBox_LogBoxId = c.Int(),
                    })
                .PrimaryKey(t => t.RowId)
                .ForeignKey("dbo.LogBoxes", t => t.LogBox_LogBoxId)
                .Index(t => t.LogBox_LogBoxId);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        TableId = c.Int(nullable: false, identity: true),
                        Pot = c.Int(nullable: false),
                        ActivePlayer_Name = c.String(maxLength: 128),
                        LogBox_LogBoxId = c.Int(),
                    })
                .PrimaryKey(t => t.TableId)
                .ForeignKey("dbo.Players", t => t.ActivePlayer_Name)
                .ForeignKey("dbo.LogBoxes", t => t.LogBox_LogBoxId)
                .Index(t => t.ActivePlayer_Name)
                .Index(t => t.LogBox_LogBoxId);
            
            CreateTable(
                "dbo.LogBoxes",
                c => new
                    {
                        LogBoxId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.LogBoxId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "LogBox_LogBoxId", "dbo.LogBoxes");
            DropForeignKey("dbo.Rows", "LogBox_LogBoxId", "dbo.LogBoxes");
            DropForeignKey("dbo.Cards", "Table_TableId", "dbo.Tables");
            DropForeignKey("dbo.Players", "Table_TableId", "dbo.Tables");
            DropForeignKey("dbo.Tables", "ActivePlayer_Name", "dbo.Players");
            DropForeignKey("dbo.Cards", "Player_Name", "dbo.Players");
            DropIndex("dbo.Tables", new[] { "LogBox_LogBoxId" });
            DropIndex("dbo.Tables", new[] { "ActivePlayer_Name" });
            DropIndex("dbo.Rows", new[] { "LogBox_LogBoxId" });
            DropIndex("dbo.Players", new[] { "Table_TableId" });
            DropIndex("dbo.Cards", new[] { "Table_TableId" });
            DropIndex("dbo.Cards", new[] { "Player_Name" });
            DropTable("dbo.LogBoxes");
            DropTable("dbo.Tables");
            DropTable("dbo.Rows");
            DropTable("dbo.Players");
            DropTable("dbo.Cards");
        }
    }
}
