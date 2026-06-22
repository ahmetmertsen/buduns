using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogapp_server.Persistence.Migrations
{
    public partial class mig_20_user_profile_privacy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFullNameVisible",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFullNameVisible",
                table: "AspNetUsers");

        }
    }
}
