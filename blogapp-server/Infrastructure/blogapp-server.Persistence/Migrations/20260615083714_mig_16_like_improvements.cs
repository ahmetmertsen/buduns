using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogapp_server.Persistence.Migrations
{
    public partial class mig_16_like_improvements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"Likes\" WHERE \"Id\" IN (SELECT \"Id\" FROM (SELECT \"Id\", ROW_NUMBER() OVER (PARTITION BY \"UserId\", \"PostId\" ORDER BY \"isActive\" DESC, \"isDeleted\" ASC, \"CreatedAt\" DESC, \"Id\" DESC) AS row_number FROM \"Likes\") AS duplicates WHERE duplicates.row_number > 1);");

            migrationBuilder.DropIndex(name: "IX_Likes_UserId", table: "Likes");

            migrationBuilder.CreateIndex(name: "IX_Notifications_Type_UserId_ActorUserId_PostId_CreatedAt", table: "Notifications", columns: new[] { "Type", "UserId", "ActorUserId", "PostId", "CreatedAt" });

            migrationBuilder.CreateIndex(name: "UX_Likes_UserId_PostId", table: "Likes", columns: new[] { "UserId", "PostId" }, unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Notifications_Type_UserId_ActorUserId_PostId_CreatedAt", table: "Notifications");

            migrationBuilder.DropIndex(name: "UX_Likes_UserId_PostId", table: "Likes");

            migrationBuilder.CreateIndex(name: "IX_Likes_UserId", table: "Likes", column: "UserId");
        }
    }
}
