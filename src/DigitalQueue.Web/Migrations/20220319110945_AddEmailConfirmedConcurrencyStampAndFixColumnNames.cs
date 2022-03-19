using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddEmailConfirmedConcurrencyStampAndFixColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "users",
                newName: "email_confirmed");

            migrationBuilder.AddColumn<string>(
                name: "concurrency_stamp",
                table: "users",
                type: "TEXT",
                nullable: true);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "concurrency_stamp",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                table: "users",
                newName: "is_active");
            
        }
    }
}
