namespace _99meat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DeliveryTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DeliveryTime", c => c.Int(nullable: false));
        }
    }
}
