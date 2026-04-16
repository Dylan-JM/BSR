using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSR.Migrations
{
    /// <inheritdoc />
    public partial class lastestfixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "a1b2c3d4-e5f6-7890-abcd-ef1234567890");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "sales",
                column: "ConcurrencyStamp",
                value: "b2c3d4e5-f6a7-8901-bcde-f12345678901");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "c3d4e5f6-a7b8-9012-cdef-123456789012");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "d5ea381a-51d2-4344-9a96-29a0180c3966");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "sales",
                column: "ConcurrencyStamp",
                value: "e5c51e7a-259f-470d-973a-d04a613bda3b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user",
                column: "ConcurrencyStamp",
                value: "6f881ef8-2e8d-4a15-a487-5e49017e5057");
        }
    }
}
