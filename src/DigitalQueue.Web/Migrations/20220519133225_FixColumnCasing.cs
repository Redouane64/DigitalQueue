using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class FixColumnCasing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sessions_users_UserId",
                table: "sessions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "sessions",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_sessions_UserId_security_stamp",
                table: "sessions",
                newName: "IX_sessions_user_id_security_stamp");

            migrationBuilder.AddForeignKey(
                name: "FK_sessions_users_user_id",
                table: "sessions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sessions_users_user_id",
                table: "sessions");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "sessions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_sessions_user_id_security_stamp",
                table: "sessions",
                newName: "IX_sessions_UserId_security_stamp");

            migrationBuilder.AddForeignKey(
                name: "FK_sessions_users_UserId",
                table: "sessions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
