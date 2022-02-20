using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddCourseAndTeacherRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "311e3bee-7fe8-4e3c-8203-7fe30c77566b");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "58a18e36-3523-4fef-8552-fa3cb6080895");

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
                name: "course_teacher",
                columns: table => new
                {
                    course_id = table.Column<string>(type: "TEXT", nullable: false),
                    teacher_id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_teacher", x => new { x.course_id, x.teacher_id });
                    table.ForeignKey(
                        name: "FK_course_teacher_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_teacher_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "247a25f7-deb5-44f2-8226-c59ac94e990d", "user", "USER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "5fece3b3-bb3e-4c4a-aefa-6e506a5eb4f1", "administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_course_teacher_teacher_id",
                table: "course_teacher",
                column: "teacher_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_teacher");

            migrationBuilder.DropTable(
                name: "courses");

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
                values: new object[] { "311e3bee-7fe8-4e3c-8203-7fe30c77566b", "user", "USER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "58a18e36-3523-4fef-8552-fa3cb6080895", "administrator", "ADMINISTRATOR" });
        }
    }
}
