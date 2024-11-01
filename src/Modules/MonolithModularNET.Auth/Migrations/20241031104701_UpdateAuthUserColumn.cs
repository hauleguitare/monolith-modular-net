using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonolithModularNET.Auth.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuthUserColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "moderator",
                column: "ConcurrencyStamp",
                value: "3c3d0036-8bea-47e1-9399-9906780044be");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "new_user",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "a90147c5-fd00-41b6-a5e7-c0df0d21ffed", "NEW MODELS" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner",
                column: "ConcurrencyStamp",
                value: "445a335e-4539-4c9c-bad2-8da2675df054");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "super_administrator",
                column: "ConcurrencyStamp",
                value: "ee09fae1-e8e1-4ce1-a3e9-5dff50d58c0b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "root",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash" },
                values: new object[] { "69ba3ff0-1496-4bcf-b617-79abe3e15615", null, null, "AQAAAAIAAYagAAAAEOsj7Ju5QiAWdq3YPn0zs1QR3OVc4wq/gHnlut8H1Cxx7qJDbvJHo4Asqsd6t6Iaeg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "moderator",
                column: "ConcurrencyStamp",
                value: "5935436e-1656-40da-8a85-679a01dcc6b6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "new_user",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "8e147d6f-d6a2-41ef-9920-6c7eb1796dfc", "NEW USER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner",
                column: "ConcurrencyStamp",
                value: "140c8093-24e7-4ec1-a19a-ea21a3244194");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "super_administrator",
                column: "ConcurrencyStamp",
                value: "ca98498a-57de-4602-8e9b-b150b7d0c8ff");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "root",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7ded8d96-0a1e-4e57-9e22-035d2128bac1", "AQAAAAIAAYagAAAAEFjWqpDzVSl+onI9p67xXY+iwHxhv+C2RaoQ6g/DR/JsTCTcQ+9A+7rCR8id1k7BHA==" });
        }
    }
}
