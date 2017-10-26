namespace KinderArtikelBoerse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sellers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Surname = c.String(maxLength: 4000),
                        FamilientreffPercentage = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemIdentifier = c.String(maxLength: 4000),
                        Description = c.String(maxLength: 4000),
                        Size = c.String(maxLength: 4000),
                        Price = c.Single(nullable: false),
                        IsSold = c.Boolean(nullable: false),
                        Seller_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sellers", t => t.Seller_Id)
                .Index(t => t.Seller_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Seller_Id", "dbo.Sellers");
            DropIndex("dbo.Items", new[] { "Seller_Id" });
            DropTable("dbo.Items");
            DropTable("dbo.Sellers");
        }
    }
}
