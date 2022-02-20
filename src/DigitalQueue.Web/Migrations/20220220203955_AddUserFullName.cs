using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddUserFullName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "07d5aee2-5a98-4043-9e10-07623fb0a456");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "0c984ba6-045f-4ef9-b681-187e2e17aa85");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "4ce325d4-9e26-409c-889f-97c72a3c92a2");

            migrationBuilder.AddColumn<string>(
                name: "fullname",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "311e3bee-7fe8-4e3c-8203-7fe30c77566b", "user", "USER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "58a18e36-3523-4fef-8552-fa3cb6080895", "administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "311e3bee-7fe8-4e3c-8203-7fe30c77566b");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "58a18e36-3523-4fef-8552-fa3cb6080895");

            migrationBuilder.DropColumn(
                name: "fullname",
                table: "users");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "07d5aee2-5a98-4043-9e10-07623fb0a456", "administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "0c984ba6-045f-4ef9-b681-187e2e17aa85", "student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "4ce325d4-9e26-409c-889f-97c72a3c92a2", "teacher", "TEACHER" });
        }
    }
}
