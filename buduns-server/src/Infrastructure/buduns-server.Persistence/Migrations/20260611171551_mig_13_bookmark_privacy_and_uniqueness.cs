using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace buduns_server.Persistence.Migrations
{
    public partial class mig_13_bookmark_privacy_and_uniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookmarks_UserId",
                table: "Bookmarks");

            migrationBuilder.Sql(
                @"WITH ranked_bookmarks AS (
                    SELECT ""Id"",
                           ROW_NUMBER() OVER (
                               PARTITION BY ""UserId"", ""PostId""
                               ORDER BY ""isDeleted"", ""isActive"" DESC, ""CreatedAt"" DESC, ""Id"" DESC) AS row_number
                    FROM ""Bookmarks""
                )
                DELETE FROM ""Bookmarks"" AS bookmark
                USING ranked_bookmarks
                WHERE bookmark.""Id"" = ranked_bookmarks.""Id""
                  AND ranked_bookmarks.row_number > 1;");

            migrationBuilder.CreateIndex(
                name: "UX_Bookmarks_UserId_PostId",
                table: "Bookmarks",
                columns: new[] { "UserId", "PostId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Bookmarks_UserId_PostId",
                table: "Bookmarks");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_UserId",
                table: "Bookmarks",
                column: "UserId");
        }
    }
}
