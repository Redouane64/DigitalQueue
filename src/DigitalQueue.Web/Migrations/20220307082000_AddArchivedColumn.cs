using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddArchivedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "9ecf8929-a1bc-4b27-a67d-c3b4a42a9204");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "dd6cf5d2-62db-40a6-a61b-6cb13f09df96");

            migrationBuilder.AddColumn<bool>(
                name: "archived",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "archived",
                table: "courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "3951323d-4e19-446d-89c8-08217a99abf5", "administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "795a3949-bf9a-45ef-9cd6-b7e43898eb9f", "user", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "3951323d-4e19-446d-89c8-08217a99abf5");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "795a3949-bf9a-45ef-9cd6-b7e43898eb9f");

            migrationBuilder.DropColumn(
                name: "archived",
                table: "users");

            migrationBuilder.DropColumn(
                name: "archived",
                table: "courses");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "9ecf8929-a1bc-4b27-a67d-c3b4a42a9204", "user", "USER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "dd6cf5d2-62db-40a6-a61b-6cb13f09df96", "administrator", "ADMINISTRATOR" });
        }
    }
}
