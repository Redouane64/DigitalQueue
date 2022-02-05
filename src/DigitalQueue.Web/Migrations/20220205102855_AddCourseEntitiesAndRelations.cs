using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddCourseEntitiesAndRelations : Migration
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

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Membership",
                columns: table => new
                {
                    course_id = table.Column<string>(type: "TEXT", nullable: false),
                    user_id = table.Column<string>(type: "TEXT", nullable: false),
                    is_teacher = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => new { x.course_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_Membership_Course_course_id",
                        column: x => x.course_id,
                        principalTable: "Course",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Membership_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "63133fd1-65b8-435e-877d-43b50846671e", "teacher", "TEACHER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "77ec1cd5-a8a9-40ad-aa41-35e48830715f", "student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "9a102baa-67ac-4b04-af3c-e8e09c5c9fff", "administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_Membership_user_id",
                table: "Membership",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Membership");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "63133fd1-65b8-435e-877d-43b50846671e");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "77ec1cd5-a8a9-40ad-aa41-35e48830715f");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "9a102baa-67ac-4b04-af3c-e8e09c5c9fff");

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
