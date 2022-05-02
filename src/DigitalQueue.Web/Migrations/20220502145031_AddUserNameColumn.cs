using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddUserNameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "users");
        }
    }
}
