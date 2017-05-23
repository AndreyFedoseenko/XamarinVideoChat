namespace DeviseSpecificAppServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChatId = c.Int(nullable: false),
                        SenderId = c.Int(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.SenderId, cascadeDelete: true)
                .Index(t => t.ChatId)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserChats",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Chat_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Chat_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Chats", t => t.Chat_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Chat_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.UserChats", "Chat_Id", "dbo.Chats");
            DropForeignKey("dbo.UserChats", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "ChatId", "dbo.Chats");
            DropIndex("dbo.UserChats", new[] { "Chat_Id" });
            DropIndex("dbo.UserChats", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Messages", new[] { "ChatId" });
            DropTable("dbo.UserChats");
            DropTable("dbo.Users");
            DropTable("dbo.Messages");
            DropTable("dbo.Chats");
        }
    }
}
