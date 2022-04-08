using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddRequestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "requests",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    course_id = table.Column<string>(type: "TEXT", nullable: false),
                    creator_id = table.Column<string>(type: "TEXT", nullable: false),
                    completed = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    create_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_requests_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_requests_users_creator_id",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_requests_course_id",
                table: "requests",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_requests_creator_id",
                table: "requests",
                column: "creator_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "requests");

        }
    }
}
