namespace PublicServicesDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(maxLength: 20),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArbitraryAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Total = c.Double(nullable: false),
                        DatePaid = c.String(),
                        Address_Id = c.Int(),
                        Rate_Id = c.Int(),
                        Service_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .ForeignKey("dbo.Rates", t => t.Rate_Id)
                .ForeignKey("dbo.Services", t => t.Service_Id)
                .Index(t => t.Address_Id)
                .Index(t => t.Rate_Id)
                .Index(t => t.Service_Id);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        MeasureTitle = c.String(),
                        Price = c.Double(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.VolumeIndications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrevIndication = c.Double(nullable: false),
                        CurIndication = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        DatePaid = c.String(),
                        Address_Id = c.Int(),
                        Rate_Id = c.Int(),
                        Service_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .ForeignKey("dbo.Rates", t => t.Rate_Id)
                .ForeignKey("dbo.Services", t => t.Service_Id)
                .Index(t => t.Address_Id)
                .Index(t => t.Rate_Id)
                .Index(t => t.Service_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VolumeIndications", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.VolumeIndications", "Rate_Id", "dbo.Rates");
            DropForeignKey("dbo.VolumeIndications", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.ArbitraryAmounts", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.Services", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ArbitraryAmounts", "Rate_Id", "dbo.Rates");
            DropForeignKey("dbo.Rates", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ArbitraryAmounts", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Addresses", "User_Id", "dbo.Users");
            DropIndex("dbo.VolumeIndications", new[] { "Service_Id" });
            DropIndex("dbo.VolumeIndications", new[] { "Rate_Id" });
            DropIndex("dbo.VolumeIndications", new[] { "Address_Id" });
            DropIndex("dbo.Services", new[] { "User_Id" });
            DropIndex("dbo.Rates", new[] { "User_Id" });
            DropIndex("dbo.ArbitraryAmounts", new[] { "Service_Id" });
            DropIndex("dbo.ArbitraryAmounts", new[] { "Rate_Id" });
            DropIndex("dbo.ArbitraryAmounts", new[] { "Address_Id" });
            DropIndex("dbo.Addresses", new[] { "User_Id" });
            DropTable("dbo.VolumeIndications");
            DropTable("dbo.Services");
            DropTable("dbo.Rates");
            DropTable("dbo.ArbitraryAmounts");
            DropTable("dbo.Users");
            DropTable("dbo.Addresses");
        }
    }
}
