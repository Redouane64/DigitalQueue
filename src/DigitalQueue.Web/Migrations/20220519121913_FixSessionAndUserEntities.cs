using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class FixSessionAndUserEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "concurrency_stamp",
                table: "users");

            migrationBuilder.DropColumn(
                name: "security_stamp",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "device_ip",
                table: "sessions",
                newName: "security_stamp");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_security_stamp",
                table: "sessions",
                column: "security_stamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sessions_security_stamp",
                table: "sessions");

            migrationBuilder.RenameColumn(
                name: "security_stamp",
                table: "sessions",
                newName: "device_ip");

            migrationBuilder.AddColumn<string>(
                name: "concurrency_stamp",
                table: "users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "security_stamp",
                table: "users",
                type: "TEXT",
                nullable: true);
        }
    }
}
