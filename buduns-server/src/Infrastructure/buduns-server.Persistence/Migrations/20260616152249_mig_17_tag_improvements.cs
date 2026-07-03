using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace buduns_server.Persistence.Migrations
{
    public partial class mig_17_tag_improvements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Tags",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE \"Tags\" SET \"Name\" = LEFT(trim(regexp_replace(COALESCE(\"Name\", ''), '\\s+', ' ', 'g')), 100);");
            migrationBuilder.Sql("UPDATE \"Tags\" SET \"NormalizedName\" = LEFT(upper(\"Name\"), 100);");
            migrationBuilder.Sql("WITH duplicate_tags AS (SELECT \"Id\", MIN(\"Id\") OVER (PARTITION BY \"NormalizedName\") AS keep_id FROM \"Tags\" WHERE \"isDeleted\" = false AND \"isActive\" = true) INSERT INTO \"PostTag\" (\"PostsId\", \"TagsId\") SELECT DISTINCT post_tag.\"PostsId\", duplicate_tags.keep_id FROM \"PostTag\" post_tag INNER JOIN duplicate_tags ON post_tag.\"TagsId\" = duplicate_tags.\"Id\" WHERE duplicate_tags.\"Id\" <> duplicate_tags.keep_id ON CONFLICT DO NOTHING;");
            migrationBuilder.Sql("WITH duplicate_tags AS (SELECT \"Id\", MIN(\"Id\") OVER (PARTITION BY \"NormalizedName\") AS keep_id FROM \"Tags\" WHERE \"isDeleted\" = false AND \"isActive\" = true) DELETE FROM \"PostTag\" post_tag USING duplicate_tags WHERE post_tag.\"TagsId\" = duplicate_tags.\"Id\" AND duplicate_tags.\"Id\" <> duplicate_tags.keep_id;");
            migrationBuilder.Sql("WITH duplicate_tags AS (SELECT \"Id\", MIN(\"Id\") OVER (PARTITION BY \"NormalizedName\") AS keep_id FROM \"Tags\" WHERE \"isDeleted\" = false AND \"isActive\" = true) UPDATE \"Tags\" tag SET \"isActive\" = false, \"isDeleted\" = true, \"UpdateAt\" = NOW() FROM duplicate_tags WHERE tag.\"Id\" = duplicate_tags.\"Id\" AND duplicate_tags.\"Id\" <> duplicate_tags.keep_id;");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "UX_Tags_NormalizedName_Active",
                table: "Tags",
                column: "NormalizedName",
                unique: true,
                filter: "\"isDeleted\" = false AND \"isActive\" = true");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Tags_NormalizedName_Active",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Tags");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
