namespace _99meat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderDetails", "Product_Id", "dbo.Products");
            DropIndex("dbo.OrderDetails", new[] { "Product_Id" });
            AddColumn("dbo.OrderDetails", "ProductId", c => c.Int(nullable: false));
            DropColumn("dbo.OrderDetails", "Product_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "Product_Id", c => c.Int());
            DropColumn("dbo.OrderDetails", "ProductId");
            CreateIndex("dbo.OrderDetails", "Product_Id");
            AddForeignKey("dbo.OrderDetails", "Product_Id", "dbo.Products", "Id");
        }
    }
}
