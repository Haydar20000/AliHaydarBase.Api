using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AliHaydarBase.Api.HelperFunctions.SeedConfiguration
{

    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(x => x.Id);

            // Seed data (converted from MemberDetails → Member)
            builder.HasData(
                new Member
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    FullNameArabic = "Ali Haydar",
                    Stage = "Stage 1",
                    RegisterNumber = "REG-001",
                    City = "Baghdad",
                    Phone = "07700000001",
                    Email = "ali.haydar@example.com",
                    Gender = "Male",
                    DateOfBirth = DateTime.Parse("1990-05-15"),
                    LastYearIdentityRenewal = DateTime.Parse("2023-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/men/1.jpg",
                    IdNumber = "ID-001",
                    JobTitle = "Manager",
                    RegisterDate = DateTime.Parse("2020-01-01")
                },
                new Member
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    FullNameArabic = "Sara Mahmood",
                    Stage = "Stage 2",
                    RegisterNumber = "REG-002",
                    City = "Basra",
                    Phone = "07700000002",
                    Email = "sara.mahmood@example.com",
                    Gender = "Female",
                    DateOfBirth = DateTime.Parse("1992-03-12"),
                    LastYearIdentityRenewal = DateTime.Parse("2024-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/women/2.jpg",
                    IdNumber = "ID-002",
                    JobTitle = "Software Engineer",
                    RegisterDate = DateTime.Parse("2021-02-15")
                },
                new Member
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    FullNameArabic = "Omar Khalid",
                    Stage = "Stage 3",
                    RegisterNumber = "REG-003",
                    City = "Mosul",
                    Phone = "07700000003",
                    Email = "omar.khalid@example.com",
                    Gender = "Male",
                    DateOfBirth = DateTime.Parse("1988-07-21"),
                    LastYearIdentityRenewal = DateTime.Parse("2023-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/men/3.jpg",
                    IdNumber = "ID-003",
                    JobTitle = "Accountant",
                    RegisterDate = DateTime.Parse("2022-03-10")
                },
                new Member
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    FullNameArabic = "Lina Ahmed",
                    Stage = "Stage 1",
                    RegisterNumber = "REG-004",
                    City = "Baghdad",
                    Phone = "07700000004",
                    Email = "lina.ahmed@example.com",
                    Gender = "Female",
                    DateOfBirth = DateTime.Parse("1995-11-05"),
                    LastYearIdentityRenewal = DateTime.Parse("2024-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/women/4.jpg",
                    IdNumber = "ID-004",
                    JobTitle = "HR Specialist",
                    RegisterDate = DateTime.Parse("2023-04-20")
                },
                new Member
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    FullNameArabic = "Hassan Jabar",
                    Stage = "Stage 2",
                    RegisterNumber = "REG-005",
                    City = "Najaf",
                    Phone = "07700000005",
                    Email = "hassan.jabar@example.com",
                    Gender = "Male",
                    DateOfBirth = DateTime.Parse("1991-09-14"),
                    LastYearIdentityRenewal = DateTime.Parse("2022-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/men/5.jpg",
                    IdNumber = "ID-005",
                    JobTitle = "Financial Analyst",
                    RegisterDate = DateTime.Parse("2022-05-15")
                },
                new Member
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    FullNameArabic = "Noor Al‑Zahra",
                    Stage = "Stage 3",
                    RegisterNumber = "REG-006",
                    City = "Karbala",
                    Phone = "07700000006",
                    Email = "noor.zahra@example.com",
                    Gender = "Female",
                    DateOfBirth = DateTime.Parse("1993-02-18"),
                    LastYearIdentityRenewal = DateTime.Parse("2024-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/women/6.jpg",
                    IdNumber = "ID-006",
                    JobTitle = "Marketing Manager",
                    RegisterDate = DateTime.Parse("2023-06-18")
                },
                new Member
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    FullNameArabic = "Mustafa Ali",
                    Stage = "Stage 1",
                    RegisterNumber = "REG-007",
                    City = "Baghdad",
                    Phone = "07700000007",
                    Email = "mustafa.ali@example.com",
                    Gender = "Male",
                    DateOfBirth = DateTime.Parse("1989-06-30"),
                    LastYearIdentityRenewal = DateTime.Parse("2023-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/men/7.jpg",
                    IdNumber = "ID-007",
                    JobTitle = "Software Developer",
                    RegisterDate = DateTime.Parse("2021-07-12")
                },
                new Member
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    FullNameArabic = "Fatima Kareem",
                    Stage = "Stage 2",
                    RegisterNumber = "REG-008",
                    City = "Basra",
                    Phone = "07700000008",
                    Email = "fatima.kareem@example.com",
                    Gender = "Female",
                    DateOfBirth = DateTime.Parse("1994-04-22"),
                    LastYearIdentityRenewal = DateTime.Parse("2024-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/women/8.jpg",
                    IdNumber = "ID-008",
                    JobTitle = "Project Manager",
                    RegisterDate = DateTime.Parse("2022-08-25")
                },
                new Member
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    FullNameArabic = "Yasir Salman",
                    Stage = "Stage 3",
                    RegisterNumber = "REG-009",
                    City = "Erbil",
                    Phone = "07700000009",
                    Email = "yasir.salman@example.com",
                    Gender = "Male",
                    DateOfBirth = DateTime.Parse("1990-12-10"),
                    LastYearIdentityRenewal = DateTime.Parse("2024-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/men/9.jpg",
                    IdNumber = "ID-009",
                    JobTitle = "Business Analyst",
                    RegisterDate = DateTime.Parse("2023-09-05")
                },
                new Member
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    FullNameArabic = "Rana Saeed",
                    Stage = "Stage 1",
                    RegisterNumber = "REG-010",
                    City = "Sulaymaniyah",
                    Phone = "07700000010",
                    Email = "rana.saeed@example.com",
                    Gender = "Female",
                    DateOfBirth = DateTime.Parse("1996-08-17"),
                    LastYearIdentityRenewal = DateTime.Parse("2023-01-05"),
                    ImageUrl = "https://randomuser.me/api/portraits/women/10.jpg",
                    IdNumber = "ID-010",
                    JobTitle = "Designer",
                    RegisterDate = DateTime.Parse("2023-10-12")
                }
            );
        }
    }

}