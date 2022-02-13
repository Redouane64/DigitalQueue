using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddCoursesNewRolesAndUserFullName : Migration
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

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "memberships",
                columns: table => new
                {
                    course_id = table.Column<string>(type: "TEXT", nullable: false),
                    user_id = table.Column<string>(type: "TEXT", nullable: false),
                    is_teacher = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_memberships", x => new { x.course_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_memberships_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_memberships_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "237ea819-ab1a-473e-b9bb-843b299aaede", "user", "USER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "75950bfe-d076-4431-ab83-8fc62959013e", "administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_memberships_user_id",
                table: "memberships",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "memberships");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "237ea819-ab1a-473e-b9bb-843b299aaede");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "75950bfe-d076-4431-ab83-8fc62959013e");

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
