using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AliHaydarBase.Api.Migrations
{
    /// <inheritdoc />
    public partial class changeseedGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a1111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "u1111111-uuuu-uuuu-uuuu-uuuuuuuuuuuu" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "u1111111-uuuu-uuuu-uuuu-uuuuuuuuuuuu");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { "d2f3c8a1-4b2e-4f9a-9b1c-8e3f2a7c9d11", null, "For App Admin", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "ForthName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "SureName", "ThirdName", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f1613dc4-c2ee-2344-b8aa-1105b4f6f1b9", 0, null, "09cb0259-1714-4576-9a22-fca5b700817e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hay19732000@gmail.com", true, "HAYDER", null, "ALZEYAD", false, null, "HAY19732000@GMAIL.COM", "HAY19732000@GMAIL.COM", "AQAAAAIAAYagAAAAEBHVJRSiK294d/4cDFS9xZ7GGNdlMo4Zqghw+JpfbTdRP4z2YlMflon/4iSVeZT//w==", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "UABF5YCL37DBZSJNMXTOXZ4QMQ3BEQRS", null, null, false, "hay19732000@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d2f3c8a1-4b2e-4f9a-9b1c-8e3f2a7c9d11", "f1613dc4-c2ee-2344-b8aa-1105b4f6f1b9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d2f3c8a1-4b2e-4f9a-9b1c-8e3f2a7c9d11", "f1613dc4-c2ee-2344-b8aa-1105b4f6f1b9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d2f3c8a1-4b2e-4f9a-9b1c-8e3f2a7c9d11");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f1613dc4-c2ee-2344-b8aa-1105b4f6f1b9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { "a1111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa", null, "For App Admin", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "ForthName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "SureName", "ThirdName", "TwoFactorEnabled", "UserName" },
                values: new object[] { "u1111111-uuuu-uuuu-uuuu-uuuuuuuuuuuu", 0, null, "09cb0259-1714-4576-9a22-fca5b700817e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hay19732000@gmail.com", true, "HAYDER", null, "ALZEYAD", false, null, "HAY19732000@GMAIL.COM", "HAY19732000@GMAIL.COM", "AQAAAAIAAYagAAAAEBHVJRSiK294d/4cDFS9xZ7GGNdlMo4Zqghw+JpfbTdRP4z2YlMflon/4iSVeZT//w==", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "UABF5YCL37DBZSJNMXTOXZ4QMQ3BEQRS", null, null, false, "hay19732000@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a1111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "u1111111-uuuu-uuuu-uuuu-uuuuuuuuuuuu" });
        }
    }
}
