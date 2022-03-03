using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class MakeCourseNameAndYearUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_courses_title",
                table: "courses");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "998be51d-29ab-4048-8ba0-f0488b4ffcf9");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "b3d5c7a0-7301-4acf-a1d5-70c3521c59fd");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "courses",
                newName: "year");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "9ecf8929-a1bc-4b27-a67d-c3b4a42a9204", "user", "USER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "dd6cf5d2-62db-40a6-a61b-6cb13f09df96", "administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_courses_title_year",
                table: "courses",
                columns: new[] { "title", "year" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_courses_title_year",
                table: "courses");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "9ecf8929-a1bc-4b27-a67d-c3b4a42a9204");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "dd6cf5d2-62db-40a6-a61b-6cb13f09df96");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "courses",
                newName: "Year");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "998be51d-29ab-4048-8ba0-f0488b4ffcf9", "user", "USER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "b3d5c7a0-7301-4acf-a1d5-70c3521c59fd", "administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_courses_title",
                table: "courses",
                column: "title",
                unique: true);
        }
    }
}
