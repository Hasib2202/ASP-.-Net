namespace IntroAPI2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class API1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        SId = c.Int(nullable: false, identity: true),
                        SName = c.String(),
                        SCgpa = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.SId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TId = c.Int(nullable: false, identity: true),
                        TName = c.String(),
                        TDesignation = c.String(),
                    })
                .PrimaryKey(t => t.TId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Teachers");
            DropTable("dbo.Students");
        }
    }
}
