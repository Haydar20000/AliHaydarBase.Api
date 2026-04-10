using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AliHaydarBase.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdCardTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "IdCardTemplates",
                columns: new[] { "Id", "BackImage", "BackLayoutJson", "CardHeightMm", "CardWidthMm", "CompanyName", "CreatedAt", "Dpi", "FrontImage", "FrontLayoutJson", "IsActive", "TemplateName", "UpdatedAt", "Year" },
                values: new object[] { new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), null, "{}", 54, 86, "Ali Haydar Co.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 300, null, "{}", true, "Default Template", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2024 });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "City", "DateOfBirth", "Email", "FullName", "Gender", "ImageUrl", "LastYearIdentityRenewal", "Phone", "RegisterNumber", "Stage", "Status" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Baghdad", "1990-01-01", "ali.haydar@example.com", "Ali Haydar", "Male", "https://randomuser.me/api/portraits/men/1.jpg", "2024", "07700000001", "REG-001", "Stage 1", "Active" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Basra", "1992-03-12", "sara.mahmood@example.com", "Sara Mahmood", "Female", "https://randomuser.me/api/portraits/women/2.jpg", "2024", "07700000002", "REG-002", "Stage 2", "Active" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Mosul", "1988-07-21", "omar.khalid@example.com", "Omar Khalid", "Male", "https://randomuser.me/api/portraits/men/3.jpg", "2023", "07700000003", "REG-003", "Stage 3", "Inactive" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Baghdad", "1995-11-05", "lina.ahmed@example.com", "Lina Ahmed", "Female", "https://randomuser.me/api/portraits/women/4.jpg", "2024", "07700000004", "REG-004", "Stage 1", "Active" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Najaf", "1991-09-14", "hassan.jabar@example.com", "Hassan Jabar", "Male", "https://randomuser.me/api/portraits/men/5.jpg", "2022", "07700000005", "REG-005", "Stage 2", "Active" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Karbala", "1993-02-18", "noor.zahra@example.com", "Noor Al‑Zahra", "Female", "https://randomuser.me/api/portraits/women/6.jpg", "2024", "07700000006", "REG-006", "Stage 3", "Active" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Baghdad", "1989-06-30", "mustafa.ali@example.com", "Mustafa Ali", "Male", "https://randomuser.me/api/portraits/men/7.jpg", "2023", "07700000007", "REG-007", "Stage 1", "Inactive" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Basra", "1994-04-22", "fatima.kareem@example.com", "Fatima Kareem", "Female", "https://randomuser.me/api/portraits/women/8.jpg", "2024", "07700000008", "REG-008", "Stage 2", "Active" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Erbil", "1990-12-10", "yasir.salman@example.com", "Yasir Salman", "Male", "https://randomuser.me/api/portraits/men/9.jpg", "2024", "07700000009", "REG-009", "Stage 3", "Active" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Sulaymaniyah", "1996-08-17", "rana.saeed@example.com", "Rana Saeed", "Female", "https://randomuser.me/api/portraits/women/10.jpg", "2023", "07700000010", "REG-010", "Stage 1", "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdCardTemplates",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        }
    }
}
