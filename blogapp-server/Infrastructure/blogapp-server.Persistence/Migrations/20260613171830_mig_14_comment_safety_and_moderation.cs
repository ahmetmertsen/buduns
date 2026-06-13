using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogapp_server.Persistence.Migrations
{
    public partial class mig_14_comment_safety_and_moderation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Reports_Target",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_ModerationActions_TargetType_TargetPostId_TargetUserId",
                table: "ModerationActions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ModerationActions_Target",
                table: "ModerationActions");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "TargetCommentId",
                table: "Reports",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActorUserId",
                table: "Notifications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Notifications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Notifications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadAt",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetCommentId",
                table: "ModerationActions",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""Comments"" SET ""Content"" = LEFT(""Content"", 1000) WHERE LENGTH(""Content"") > 1000;");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterUserId_TargetCommentId",
                table: "Reports",
                columns: new[] { "ReporterUserId", "TargetCommentId" },
                unique: true,
                filter: "\"TargetType\" = 2 AND \"Status\" IN (1, 2)");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TargetCommentId",
                table: "Reports",
                column: "TargetCommentId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reports_Target",
                table: "Reports",
                sql: "(\"TargetType\" = 0 AND \"TargetPostId\" IS NOT NULL AND \"TargetUserId\" IS NULL AND \"TargetCommentId\" IS NULL) OR (\"TargetType\" = 1 AND \"TargetPostId\" IS NULL AND \"TargetUserId\" IS NOT NULL AND \"TargetCommentId\" IS NULL) OR (\"TargetType\" = 2 AND \"TargetPostId\" IS NULL AND \"TargetUserId\" IS NULL AND \"TargetCommentId\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ActorUserId",
                table: "Notifications",
                column: "ActorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CommentId",
                table: "Notifications",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PostId",
                table: "Notifications",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead_CreatedAt",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ModerationActions_TargetCommentId",
                table: "ModerationActions",
                column: "TargetCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationActions_TargetType_TargetPostId_TargetUserId_Targ~",
                table: "ModerationActions",
                columns: new[] { "TargetType", "TargetPostId", "TargetUserId", "TargetCommentId" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_ModerationActions_Target",
                table: "ModerationActions",
                sql: "(\"TargetType\" = 0 AND \"TargetPostId\" IS NOT NULL AND \"TargetCommentId\" IS NULL) OR (\"TargetType\" = 1 AND \"TargetPostId\" IS NULL AND \"TargetUserId\" IS NOT NULL AND \"TargetCommentId\" IS NULL) OR (\"TargetType\" = 2 AND \"TargetPostId\" IS NULL AND \"TargetCommentId\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId_Status_CreatedAt",
                table: "Comments",
                columns: new[] { "PostId", "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId_Status_CreatedAt",
                table: "Comments",
                columns: new[] { "UserId", "Status", "CreatedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationActions_Comments_TargetCommentId",
                table: "ModerationActions",
                column: "TargetCommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_ActorUserId",
                table: "Notifications",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Comments_CommentId",
                table: "Notifications",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Posts_PostId",
                table: "Notifications",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Comments_TargetCommentId",
                table: "Reports",
                column: "TargetCommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""ModerationActions"" WHERE ""TargetType"" = 2;");
            migrationBuilder.Sql(@"DELETE FROM ""Reports"" WHERE ""TargetType"" = 2;");

            migrationBuilder.DropForeignKey(
                name: "FK_ModerationActions_Comments_TargetCommentId",
                table: "ModerationActions");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_ActorUserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Comments_CommentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Posts_PostId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Comments_TargetCommentId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReporterUserId_TargetCommentId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_TargetCommentId",
                table: "Reports");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reports_Target",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ActorUserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CommentId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_PostId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId_IsRead_CreatedAt",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_ModerationActions_TargetCommentId",
                table: "ModerationActions");

            migrationBuilder.DropIndex(
                name: "IX_ModerationActions_TargetType_TargetPostId_TargetUserId_Targ~",
                table: "ModerationActions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ModerationActions_Target",
                table: "ModerationActions");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId_Status_CreatedAt",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId_Status_CreatedAt",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TargetCommentId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ActorUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ReadAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TargetCommentId",
                table: "ModerationActions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reports_Target",
                table: "Reports",
                sql: "(\"TargetType\" = 0 AND \"TargetPostId\" IS NOT NULL AND \"TargetUserId\" IS NULL) OR (\"TargetType\" = 1 AND \"TargetPostId\" IS NULL AND \"TargetUserId\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationActions_TargetType_TargetPostId_TargetUserId",
                table: "ModerationActions",
                columns: new[] { "TargetType", "TargetPostId", "TargetUserId" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_ModerationActions_Target",
                table: "ModerationActions",
                sql: "(\"TargetType\" = 0 AND \"TargetPostId\" IS NOT NULL) OR (\"TargetType\" = 1 AND \"TargetUserId\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }
    }
}
