using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class FixCourseAndMembershipEntitiesTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Membership_Course_course_id",
                table: "Membership");

            migrationBuilder.DropForeignKey(
                name: "FK_Membership_users_user_id",
                table: "Membership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Membership",
                table: "Membership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

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

            migrationBuilder.RenameTable(
                name: "Membership",
                newName: "memberships");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "courses");

            migrationBuilder.RenameIndex(
                name: "IX_Membership_user_id",
                table: "memberships",
                newName: "IX_memberships_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_memberships",
                table: "memberships",
                columns: new[] { "course_id", "user_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_courses",
                table: "courses",
                column: "id");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "22dfb2c2-f847-4afc-8bb5-7c1bb88d8217", "teacher", "TEACHER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "4fcf4a41-1724-4c2e-a384-0c50c35ac85a", "administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "b8eb1daa-8cb5-4482-a8c4-2ff8665cf970", "student", "STUDENT" });

            migrationBuilder.AddForeignKey(
                name: "FK_memberships_courses_course_id",
                table: "memberships",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_memberships_users_user_id",
                table: "memberships",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_memberships_courses_course_id",
                table: "memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_memberships_users_user_id",
                table: "memberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_memberships",
                table: "memberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_courses",
                table: "courses");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "22dfb2c2-f847-4afc-8bb5-7c1bb88d8217");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "4fcf4a41-1724-4c2e-a384-0c50c35ac85a");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "b8eb1daa-8cb5-4482-a8c4-2ff8665cf970");

            migrationBuilder.RenameTable(
                name: "memberships",
                newName: "Membership");

            migrationBuilder.RenameTable(
                name: "courses",
                newName: "Course");

            migrationBuilder.RenameIndex(
                name: "IX_memberships_user_id",
                table: "Membership",
                newName: "IX_Membership_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Membership",
                table: "Membership",
                columns: new[] { "course_id", "user_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Membership_Course_course_id",
                table: "Membership",
                column: "course_id",
                principalTable: "Course",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Membership_users_user_id",
                table: "Membership",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
