namespace PublicServicesDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdduserinVolumeIndication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VolumeIndications", "User_Id", c => c.Int());
            CreateIndex("dbo.VolumeIndications", "User_Id");
            AddForeignKey("dbo.VolumeIndications", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VolumeIndications", "User_Id", "dbo.Users");
            DropIndex("dbo.VolumeIndications", new[] { "User_Id" });
            DropColumn("dbo.VolumeIndications", "User_Id");
        }
    }
}
