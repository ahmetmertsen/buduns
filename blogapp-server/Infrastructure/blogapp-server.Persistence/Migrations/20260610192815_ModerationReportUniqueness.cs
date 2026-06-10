using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogapp_server.Persistence.Migrations
{
    public partial class ModerationReportUniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterUserId_TargetPostId",
                table: "Reports",
                columns: new[] { "ReporterUserId", "TargetPostId" },
                unique: true,
                filter: "\"TargetType\" = 0 AND \"Status\" IN (1, 2)");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterUserId_TargetUserId",
                table: "Reports",
                columns: new[] { "ReporterUserId", "TargetUserId" },
                unique: true,
                filter: "\"TargetType\" = 1 AND \"Status\" IN (1, 2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_ReporterUserId_TargetPostId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReporterUserId_TargetUserId",
                table: "Reports");
        }
    }
}
