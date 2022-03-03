using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalQueue.Web.Migrations
{
    public partial class AddYearCreatedAtUpdateAtColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "34f998be-ad1f-44cc-a687-a9a8ea8d786b");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "5cc3296a-6347-4bc5-a395-418361db2c0b");

            migrationBuilder.AddColumn<DateTime>(
                name: "create_at",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "create_at",
                table: "courses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "courses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "998be51d-29ab-4048-8ba0-f0488b4ffcf9", "user", "USER" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "b3d5c7a0-7301-4acf-a1d5-70c3521c59fd", "administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "998be51d-29ab-4048-8ba0-f0488b4ffcf9");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: "b3d5c7a0-7301-4acf-a1d5-70c3521c59fd");

            migrationBuilder.DropColumn(
                name: "create_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "create_at",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "courses");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "34f998be-ad1f-44cc-a687-a9a8ea8d786b", "administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[] { "5cc3296a-6347-4bc5-a395-418361db2c0b", "user", "USER" });
        }
    }
}
