namespace MoodExpenseTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moodexpense : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        CardId = c.Int(nullable: false, identity: true),
                        CardName = c.String(),
                        CardType = c.String(),
                    })
                .PrimaryKey(t => t.CardId);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ExpenseId = c.Int(nullable: false, identity: true),
                        ExpenseName = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        CardId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        MoodId = c.Int(nullable: false),
                        WeatherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExpenseId)
                .ForeignKey("dbo.Cards", t => t.CardId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Moods", t => t.MoodId, cascadeDelete: true)
                .ForeignKey("dbo.Weathers", t => t.WeatherId, cascadeDelete: true)
                .Index(t => t.CardId)
                .Index(t => t.CategoryId)
                .Index(t => t.MoodId)
                .Index(t => t.WeatherId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Moods",
                c => new
                    {
                        MoodId = c.Int(nullable: false, identity: true),
                        MoodName = c.String(),
                    })
                .PrimaryKey(t => t.MoodId);
            
            CreateTable(
                "dbo.Weathers",
                c => new
                    {
                        WeatherId = c.Int(nullable: false, identity: true),
                        WeatherName = c.String(),
                    })
                .PrimaryKey(t => t.WeatherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "WeatherId", "dbo.Weathers");
            DropForeignKey("dbo.Expenses", "MoodId", "dbo.Moods");
            DropForeignKey("dbo.Expenses", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Expenses", "CardId", "dbo.Cards");
            DropIndex("dbo.Expenses", new[] { "WeatherId" });
            DropIndex("dbo.Expenses", new[] { "MoodId" });
            DropIndex("dbo.Expenses", new[] { "CategoryId" });
            DropIndex("dbo.Expenses", new[] { "CardId" });
            DropTable("dbo.Weathers");
            DropTable("dbo.Moods");
            DropTable("dbo.Categories");
            DropTable("dbo.Expenses");
            DropTable("dbo.Cards");
        }
    }
}
