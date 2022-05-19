using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddSessionIndexOnUserSecurityStamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sessions_security_stamp",
                table: "sessions");

            migrationBuilder.DropIndex(
                name: "IX_sessions_UserId",
                table: "sessions");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_UserId_security_stamp",
                table: "sessions",
                columns: new[] { "UserId", "security_stamp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sessions_UserId_security_stamp",
                table: "sessions");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_security_stamp",
                table: "sessions",
                column: "security_stamp");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_UserId",
                table: "sessions",
                column: "UserId");
        }
    }
}
