namespace PublicServicesDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Datetimefordatepaid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VolumeIndications", "DatePaid", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VolumeIndications", "DatePaid", c => c.String());
        }
    }
}
