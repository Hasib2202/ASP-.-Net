namespace IntroAPI2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class API11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "SName", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "SName", c => c.String());
        }
    }
}
