namespace _99meat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DeliveryTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DeliveryTime", c => c.DateTime(nullable: false));
        }
    }
}
