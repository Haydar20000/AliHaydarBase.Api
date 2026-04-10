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
                    FullName = "Ali Haydar",
                    Stage = "Stage 1",
                    RegisterNumber = "REG-001",
                    Status = "Active",
                    City = "Baghdad",
                    Phone = "07700000001",
                    Email = "ali.haydar@example.com",
                    Gender = "Male",
                    DateOfBirth = "1990-01-01",
                    LastYearIdentityRenewal = "2024",
                    ImageUrl = "https://randomuser.me/api/portraits/men/1.jpg"
                },
                new Member
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    FullName = "Sara Mahmood",
                    Stage = "Stage 2",
                    RegisterNumber = "REG-002",
                    Status = "Active",
                    City = "Basra",
                    Phone = "07700000002",
                    Email = "sara.mahmood@example.com",
                    Gender = "Female",
                    DateOfBirth = "1992-03-12",
                    LastYearIdentityRenewal = "2024",
                    ImageUrl = "https://randomuser.me/api/portraits/women/2.jpg"
                },
                new Member
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    FullName = "Omar Khalid",
                    Stage = "Stage 3",
                    RegisterNumber = "REG-003",
                    Status = "Inactive",
                    City = "Mosul",
                    Phone = "07700000003",
                    Email = "omar.khalid@example.com",
                    Gender = "Male",
                    DateOfBirth = "1988-07-21",
                    LastYearIdentityRenewal = "2023",
                    ImageUrl = "https://randomuser.me/api/portraits/men/3.jpg"
                },
                new Member
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    FullName = "Lina Ahmed",
                    Stage = "Stage 1",
                    RegisterNumber = "REG-004",
                    Status = "Active",
                    City = "Baghdad",
                    Phone = "07700000004",
                    Email = "lina.ahmed@example.com",
                    Gender = "Female",
                    DateOfBirth = "1995-11-05",
                    LastYearIdentityRenewal = "2024",
                    ImageUrl = "https://randomuser.me/api/portraits/women/4.jpg"
                },
                new Member
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    FullName = "Hassan Jabar",
                    Stage = "Stage 2",
                    RegisterNumber = "REG-005",
                    Status = "Active",
                    City = "Najaf",
                    Phone = "07700000005",
                    Email = "hassan.jabar@example.com",
                    Gender = "Male",
                    DateOfBirth = "1991-09-14",
                    LastYearIdentityRenewal = "2022",
                    ImageUrl = "https://randomuser.me/api/portraits/men/5.jpg"
                },
                new Member
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    FullName = "Noor Al‑Zahra",
                    Stage = "Stage 3",
                    RegisterNumber = "REG-006",
                    Status = "Active",
                    City = "Karbala",
                    Phone = "07700000006",
                    Email = "noor.zahra@example.com",
                    Gender = "Female",
                    DateOfBirth = "1993-02-18",
                    LastYearIdentityRenewal = "2024",
                    ImageUrl = "https://randomuser.me/api/portraits/women/6.jpg"
                },
                new Member
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    FullName = "Mustafa Ali",
                    Stage = "Stage 1",
                    RegisterNumber = "REG-007",
                    Status = "Inactive",
                    City = "Baghdad",
                    Phone = "07700000007",
                    Email = "mustafa.ali@example.com",
                    Gender = "Male",
                    DateOfBirth = "1989-06-30",
                    LastYearIdentityRenewal = "2023",
                    ImageUrl = "https://randomuser.me/api/portraits/men/7.jpg"
                },
                new Member
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    FullName = "Fatima Kareem",
                    Stage = "Stage 2",
                    RegisterNumber = "REG-008",
                    Status = "Active",
                    City = "Basra",
                    Phone = "07700000008",
                    Email = "fatima.kareem@example.com",
                    Gender = "Female",
                    DateOfBirth = "1994-04-22",
                    LastYearIdentityRenewal = "2024",
                    ImageUrl = "https://randomuser.me/api/portraits/women/8.jpg"
                },
                new Member
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    FullName = "Yasir Salman",
                    Stage = "Stage 3",
                    RegisterNumber = "REG-009",
                    Status = "Active",
                    City = "Erbil",
                    Phone = "07700000009",
                    Email = "yasir.salman@example.com",
                    Gender = "Male",
                    DateOfBirth = "1990-12-10",
                    LastYearIdentityRenewal = "2024",
                    ImageUrl = "https://randomuser.me/api/portraits/men/9.jpg"
                },
                new Member
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    FullName = "Rana Saeed",
                    Stage = "Stage 1",
                    RegisterNumber = "REG-010",
                    Status = "Active",
                    City = "Sulaymaniyah",
                    Phone = "07700000010",
                    Email = "rana.saeed@example.com",
                    Gender = "Female",
                    DateOfBirth = "1996-08-17",
                    LastYearIdentityRenewal = "2023",
                    ImageUrl = "https://randomuser.me/api/portraits/women/10.jpg"
                }
            );
        }
    }

}