using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DateIndexCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowings_AspNetUsers_FollowerId",
                table: "UserFollowings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowings_AspNetUsers_TargetUserId",
                table: "UserFollowings");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Date",
                table: "Activities",
                column: "Date");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowings_AspNetUsers_FollowerId",
                table: "UserFollowings",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowings_AspNetUsers_TargetUserId",
                table: "UserFollowings",
                column: "TargetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowings_AspNetUsers_FollowerId",
                table: "UserFollowings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowings_AspNetUsers_TargetUserId",
                table: "UserFollowings");

            migrationBuilder.DropIndex(
                name: "IX_Activities_Date",
                table: "Activities");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowings_AspNetUsers_FollowerId",
                table: "UserFollowings",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowings_AspNetUsers_TargetUserId",
                table: "UserFollowings",
                column: "TargetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
