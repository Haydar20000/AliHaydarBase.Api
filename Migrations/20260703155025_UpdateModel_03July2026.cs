using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AliHaydarBase.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel_03July2026 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrintedBy",
                table: "PrintHistories",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "PrintedAt",
                table: "PrintHistories",
                newName: "PrintedAtUtc");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Members",
                newName: "RegisterDate");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Members",
                newName: "JobTitle");

            migrationBuilder.AddColumn<string>(
                name: "ActionType",
                table: "PrintHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BackThumbnailBase64",
                table: "PrintHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FrontThumbnailBase64",
                table: "PrintHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MemberName",
                table: "PrintHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrintMode",
                table: "PrintHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "PrintHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullNameArabic",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullNameEnglish",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdNumber",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBlockedByAdmin",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIdPrinted",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssuanceDate",
                table: "Members",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TemplateVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    FrontLayoutJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackLayoutJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PreviewImageBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateVersions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "u1111111-uuuu-uuuu-uuuu-uuuuuuuuuuuu",
                column: "City",
                value: null);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Ali Haydar", "", "ID-001", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager", "2020-01-01" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Sara Mahmood", "", "ID-002", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Engineer", "2021-02-15" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Omar Khalid", "", "ID-003", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Accountant", "2022-03-10" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Lina Ahmed", "", "ID-004", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Specialist", "2023-04-20" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Hassan Jabar", "", "ID-005", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Financial Analyst", "2022-05-15" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Noor Al‑Zahra", "", "ID-006", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing Manager", "2023-06-18" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Mustafa Ali", "", "ID-007", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Developer", "2021-07-12" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Fatima Kareem", "", "ID-008", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Project Manager", "2022-08-25" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Yasir Salman", "", "ID-009", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Analyst", "2023-09-05" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "FullNameArabic", "FullNameEnglish", "IdNumber", "IsBlockedByAdmin", "IsIdPrinted", "IssuanceDate", "JobTitle", "RegisterDate" },
                values: new object[] { "Rana Saeed", "", "ID-010", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Designer", "2023-10-12" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateVersions");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "PrintHistories");

            migrationBuilder.DropColumn(
                name: "BackThumbnailBase64",
                table: "PrintHistories");

            migrationBuilder.DropColumn(
                name: "FrontThumbnailBase64",
                table: "PrintHistories");

            migrationBuilder.DropColumn(
                name: "MemberName",
                table: "PrintHistories");

            migrationBuilder.DropColumn(
                name: "PrintMode",
                table: "PrintHistories");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "PrintHistories");

            migrationBuilder.DropColumn(
                name: "FullNameArabic",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FullNameEnglish",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IdNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IsBlockedByAdmin",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IsIdPrinted",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IssuanceDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PrintHistories",
                newName: "PrintedBy");

            migrationBuilder.RenameColumn(
                name: "PrintedAtUtc",
                table: "PrintHistories",
                newName: "PrintedAt");

            migrationBuilder.RenameColumn(
                name: "RegisterDate",
                table: "Members",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "JobTitle",
                table: "Members",
                newName: "FullName");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Ali Haydar", "Active" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Sara Mahmood", "Active" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Omar Khalid", "Inactive" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Lina Ahmed", "Active" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Hassan Jabar", "Active" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Noor Al‑Zahra", "Active" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Mustafa Ali", "Inactive" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Fatima Kareem", "Active" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Yasir Salman", "Active" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "FullName", "Status" },
                values: new object[] { "Rana Saeed", "Active" });
        }
    }
}
