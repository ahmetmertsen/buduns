using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace buduns_server.Persistence.Migrations
{
    public partial class mig_15_follower_improvements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"Followers\" WHERE \"FollowerId\" = \"FollowingId\"");

            migrationBuilder.AddCheckConstraint(name: "CK_Followers_DifferentUsers", table: "Followers", sql: "\"FollowerId\" <> \"FollowingId\"");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(name: "CK_Followers_DifferentUsers", table: "Followers");
        }
    }
}
