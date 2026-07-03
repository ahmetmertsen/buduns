using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace buduns_server.Persistence.Migrations
{
    public partial class ModerationSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The old Reviewed value (2) represented a completed review. Keep those reports closed.
            migrationBuilder.Sql("UPDATE \"Reports\" SET \"Status\" = 3 WHERE \"Status\" = 2;");

            migrationBuilder.DropColumn(
                name: "CoverImgUrl",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "SuspendedUntil",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ModerationActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportId = table.Column<int>(type: "integer", nullable: false),
                    ModeratorUserId = table.Column<int>(type: "integer", nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    TargetType = table.Column<int>(type: "integer", nullable: false),
                    TargetPostId = table.Column<int>(type: "integer", nullable: true),
                    TargetUserId = table.Column<int>(type: "integer", nullable: true),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerationActions", x => x.Id);
                    table.CheckConstraint("CK_ModerationActions_Target", "(\"TargetType\" = 0 AND \"TargetPostId\" IS NOT NULL) OR (\"TargetType\" = 1 AND \"TargetUserId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_ModerationActions_AspNetUsers_ModeratorUserId",
                        column: x => x.ModeratorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModerationActions_AspNetUsers_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModerationActions_Posts_TargetPostId",
                        column: x => x.TargetPostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModerationActions_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TargetType_TargetPostId_TargetUserId_Status",
                table: "Reports",
                columns: new[] { "TargetType", "TargetPostId", "TargetUserId", "Status" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reports_Target",
                table: "Reports",
                sql: "(\"TargetType\" = 0 AND \"TargetPostId\" IS NOT NULL AND \"TargetUserId\" IS NULL) OR (\"TargetType\" = 1 AND \"TargetPostId\" IS NULL AND \"TargetUserId\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationActions_CreatedAt",
                table: "ModerationActions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationActions_ModeratorUserId",
                table: "ModerationActions",
                column: "ModeratorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationActions_ReportId",
                table: "ModerationActions",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationActions_TargetPostId",
                table: "ModerationActions",
                column: "TargetPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationActions_TargetType_TargetPostId_TargetUserId",
                table: "ModerationActions",
                columns: new[] { "TargetType", "TargetPostId", "TargetUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_ModerationActions_TargetUserId",
                table: "ModerationActions",
                column: "TargetUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModerationActions");

            migrationBuilder.DropIndex(
                name: "IX_Reports_TargetType_TargetPostId_TargetUserId_Status",
                table: "Reports");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reports_Target",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SuspendedUntil",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "CoverImgUrl",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
