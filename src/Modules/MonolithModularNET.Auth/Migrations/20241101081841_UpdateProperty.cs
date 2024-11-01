using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonolithModularNET.Auth.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "moderator",
                column: "ConcurrencyStamp",
                value: "aafcb280-03e3-4aae-a15a-30ffc7b71a17");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "new_user",
                column: "ConcurrencyStamp",
                value: "3ac86a35-48aa-41ea-b45d-ed197a7f4df3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "owner",
                column: "ConcurrencyStamp",
                value: "194605ad-111f-42d4-9795-1c6a92083c69");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "super_administrator",
                column: "ConcurrencyStamp",
                value: "5a4666d6-09cb-4439-a174-62a115044698");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "root",
                columns: new[] { "ConcurrencyStamp", "IsActive", "PasswordHash" },
                values: new object[] { "f65e8d69-c60b-481d-a943-e4bdd81ad57d", false, "AQAAAAIAAYagAAAAEJuLeFk3fxnWYTbmkInm6EQ/AZW7tZI+mO1qocmE5PQeL098m50rlDxx9/bp9kLZHg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

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
                column: "ConcurrencyStamp",
                value: "a90147c5-fd00-41b6-a5e7-c0df0d21ffed");

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "69ba3ff0-1496-4bcf-b617-79abe3e15615", "AQAAAAIAAYagAAAAEOsj7Ju5QiAWdq3YPn0zs1QR3OVc4wq/gHnlut8H1Cxx7qJDbvJHo4Asqsd6t6Iaeg==" });
        }
    }
}
