namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        FullName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Address = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreateAt = c.DateTime(nullable: false),
                        VenderId = c.Long(nullable: false),
                        VendorName = c.String(),
                        CustomerId = c.Long(nullable: false),
                        CustomerName = c.String(),
                        Quantity = c.Double(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ResourceId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvoiceDetail",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InvoiceId = c.Long(nullable: false),
                        StockId = c.Long(nullable: false),
                        ResourceId = c.Long(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InWarehouse",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreateAt = c.DateTime(nullable: false),
                        Number = c.Int(nullable: false),
                        VenderId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InWarehouseStock",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StockId = c.Long(nullable: false),
                        InWarehouseId = c.Long(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OutWarehouse",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreateAt = c.DateTime(nullable: false),
                        Number = c.Int(nullable: false),
                        CustomerId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OutWarehouseStock",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StockId = c.Long(nullable: false),
                        OutWarehouseId = c.Long(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stock",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageURL = c.String(),
                        CategoryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        FullName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Address = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vendor",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        FullName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Address = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vendor");
            DropTable("dbo.User");
            DropTable("dbo.Stock");
            DropTable("dbo.OutWarehouseStock");
            DropTable("dbo.OutWarehouse");
            DropTable("dbo.InWarehouseStock");
            DropTable("dbo.InWarehouse");
            DropTable("dbo.InvoiceDetail");
            DropTable("dbo.Invoice");
            DropTable("dbo.Customer");
            DropTable("dbo.Category");
        }
    }
}
