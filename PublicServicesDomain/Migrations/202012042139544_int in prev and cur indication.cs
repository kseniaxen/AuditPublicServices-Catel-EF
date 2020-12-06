namespace PublicServicesDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intinprevandcurindication : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VolumeIndications", "PrevIndication", c => c.Int(nullable: false));
            AlterColumn("dbo.VolumeIndications", "CurIndication", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VolumeIndications", "CurIndication", c => c.Double(nullable: false));
            AlterColumn("dbo.VolumeIndications", "PrevIndication", c => c.Double(nullable: false));
        }
    }
}
