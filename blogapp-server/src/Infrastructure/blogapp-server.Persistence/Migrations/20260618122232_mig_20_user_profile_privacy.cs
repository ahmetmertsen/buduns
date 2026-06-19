using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogapp_server.Persistence.Migrations
{
    public partial class mig_20_user_profile_privacy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "TargetContentSnapshot",
                table: "Reports",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetOwnerFullNameSnapshot",
                table: "Reports",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetOwnerUserId",
                table: "Reports",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetOwnerUserNameSnapshot",
                table: "Reports",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "IsFullNameVisible",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Status_isPublished_isActive_isDeleted_CreatedAt",
                table: "Posts",
                columns: new[] { "Status", "isPublished", "isActive", "isDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId_Status_CreatedAt",
                table: "Posts",
                columns: new[] { "UserId", "Status", "CreatedAt" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_Status_isPublished_isActive_isDeleted_CreatedAt",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId_Status_CreatedAt",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TargetContentSnapshot",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TargetOwnerFullNameSnapshot",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TargetOwnerUserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TargetOwnerUserNameSnapshot",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IsFullNameVisible",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");
        }
    }
}
