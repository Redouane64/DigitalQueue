using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class UpdateSeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "22dfb2c2-f847-4afc-8bb5-7c1bb88d8217");
            
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "b8eb1daa-8cb5-4482-a8c4-2ff8665cf970");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "03c53d15-1be5-4c96-ae9f-f8aa4d919a8b", "user", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "03c53d15-1be5-4c96-ae9f-f8aa4d919a8b");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "cbf534bf-a935-4119-84d5-be98090946da");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "22dfb2c2-f847-4afc-8bb5-7c1bb88d8217", "teacher", "TEACHER" });
            
            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "b8eb1daa-8cb5-4482-a8c4-2ff8665cf970", "student", "STUDENT" });
        }
    }
}
