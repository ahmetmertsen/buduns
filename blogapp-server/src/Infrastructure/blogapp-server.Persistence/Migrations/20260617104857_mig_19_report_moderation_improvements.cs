using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogapp_server.Persistence.Migrations
{
    public partial class mig_19_report_moderation_improvements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_ReporterUserId_TargetType_TargetPostId_TargetUserId",
                table: "Reports");

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

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterUserId_TargetType_TargetIds",
                table: "Reports",
                columns: new[] { "ReporterUserId", "TargetType", "TargetPostId", "TargetUserId", "TargetCommentId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_ReporterUserId_TargetType_TargetIds",
                table: "Reports");

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

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterUserId_TargetType_TargetPostId_TargetUserId",
                table: "Reports",
                columns: new[] { "ReporterUserId", "TargetType", "TargetPostId", "TargetUserId" });
        }
    }
}
