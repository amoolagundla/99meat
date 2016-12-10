namespace _99meat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Favourite : DbMigration
    {
        public override void Up()
        {
            
            
            CreateTable(
                "dbo.Favourites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Email = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        DeliveryTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProdcutName = c.String(),
                        ProductDesc = c.String(),
                        IsProductActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
           
            
        }
        
        public override void Down()
        {
           
        }
    }
}
