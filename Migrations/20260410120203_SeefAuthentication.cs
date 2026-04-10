using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AliHaydarBase.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeefAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a1111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa", null, "For App Admin", "Admin", "ADMIN" },
                    { "f1603dc4-c2ee-4974-b8ca-1105b4f6f1b9", null, "For Not subscribed User", "visitor", "VISITOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "ForthName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "SureName", "ThirdName", "TwoFactorEnabled", "UserName" },
                values: new object[] { "u1111111-uuuu-uuuu-uuuu-uuuuuuuuuuuu", 0, "09cb0259-1714-4576-9a22-fca5b700817e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hay19732000@gmail.com", true, "HAYDER", null, "ALZEYAD", false, null, "HAY19732000@GMAIL.COM", "HAY19732000@GMAIL.COM", "AQAAAAIAAYagAAAAEBHVJRSiK294d/4cDFS9xZ7GGNdlMo4Zqghw+JpfbTdRP4z2YlMflon/4iSVeZT//w==", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "UABF5YCL37DBZSJNMXTOXZ4QMQ3BEQRS", null, null, false, "hay19732000@gmail.com" });

            migrationBuilder.InsertData(
                table: "ClaimDefinitions",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "Group", "IsActive", "IsVisibleToFrontend", "Scope", "Type", "UiHint" },
                values: new object[,]
                {
                    { new Guid("a1bfe781-1f6a-465b-896e-6f233376b74d"), "Access", new DateTime(2025, 10, 24, 22, 0, 0, 0, DateTimeKind.Utc), "User's assigned department", "Organizational", true, true, "UserManagement", "Department", "ShowDepartmentScopedUI" },
                    { new Guid("b2bfe781-1f6a-465b-896e-6f233376b74d"), "Access", new DateTime(2025, 10, 24, 22, 0, 0, 0, DateTimeKind.Utc), "User's security clearance level", "Organizational", true, true, "UserManagement", "SecurityLevel", "ShowSecurityLevelBadge" },
                    { new Guid("c3bfe781-1f6a-465b-896e-6f233376b74d"), "Permission", new DateTime(2025, 10, 24, 22, 0, 0, 0, DateTimeKind.Utc), "Permission to edit student grades", "Academic", true, true, "Academic", "CanEditGrades", "EnableGradeEditor" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a1111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "u1111111-uuuu-uuuu-uuuu-uuuuuuuuuuuu" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1603dc4-c2ee-4974-b8ca-1105b4f6f1b9");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a1111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "u1111111-uuuu-uuuu-uuuu-uuuuuuuuuuuu" });

            migrationBuilder.DeleteData(
                table: "ClaimDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("a1bfe781-1f6a-465b-896e-6f233376b74d"));

            migrationBuilder.DeleteData(
                table: "ClaimDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("b2bfe781-1f6a-465b-896e-6f233376b74d"));

            migrationBuilder.DeleteData(
                table: "ClaimDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("c3bfe781-1f6a-465b-896e-6f233376b74d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "u1111111-uuuu-uuuu-uuuu-uuuuuuuuuuuu");
        }
    }
}
