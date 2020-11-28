namespace PublicServicesDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class decimalmoney : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ArbitraryAmounts", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Rates", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.VolumeIndications", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VolumeIndications", "Total", c => c.Double(nullable: false));
            AlterColumn("dbo.Rates", "Price", c => c.Double(nullable: false));
            AlterColumn("dbo.ArbitraryAmounts", "Total", c => c.Double(nullable: false));
        }
    }
}
