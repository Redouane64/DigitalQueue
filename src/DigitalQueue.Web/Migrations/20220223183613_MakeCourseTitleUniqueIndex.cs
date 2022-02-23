using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class MakeCourseTitleUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "247a25f7-deb5-44f2-8226-c59ac94e990d");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "5fece3b3-bb3e-4c4a-aefa-6e506a5eb4f1");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "34f998be-ad1f-44cc-a687-a9a8ea8d786b", "administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "5cc3296a-6347-4bc5-a395-418361db2c0b", "user", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_courses_title",
                table: "courses",
                column: "title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_courses_title",
                table: "courses");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "34f998be-ad1f-44cc-a687-a9a8ea8d786b");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "5cc3296a-6347-4bc5-a395-418361db2c0b");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "247a25f7-deb5-44f2-8226-c59ac94e990d", "user", "USER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "5fece3b3-bb3e-4c4a-aefa-6e506a5eb4f1", "administrator", "ADMINISTRATOR" });
        }
    }
}
