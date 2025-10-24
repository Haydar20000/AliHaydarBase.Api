using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AliHaydarBase.Api.HelperFunctions.SeedConfiguration
{
    public class ClaimConfiguration : IEntityTypeConfiguration<ClaimDefinition>
    {
        public void Configure(EntityTypeBuilder<ClaimDefinition> builder)
        {
            builder.HasData(
             new ClaimDefinition
             {
                 Id = Guid.Parse("a1bfe781-1f6a-465b-896e-6f233376b74d"),
                 Type = "Department",
                 Description = "User's assigned department",
                 Category = "Access",
                 UiHint = "ShowDepartmentScopedUI",
                 IsVisibleToFrontend = true,
                 Scope = "UserManagement",
                 Group = "Organizational",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
                new ClaimDefinition
                {
                    Id = Guid.Parse("b2bfe781-1f6a-465b-896e-6f233376b74d"),
                    Type = "SecurityLevel",
                    Description = "User's security clearance level",
                    Category = "Access",
                    UiHint = "ShowSecurityLevelBadge",
                    IsVisibleToFrontend = true,
                    Scope = "UserManagement",
                    Group = "Organizational",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }, new ClaimDefinition
                {
                    Id = Guid.Parse("c3bfe781-1f6a-465b-896e-6f233376b74d"),
                    Type = "CanEditGrades",
                    Description = "Permission to edit student grades",
                    Category = "Permission",
                    UiHint = "EnableGradeEditor",
                    IsVisibleToFrontend = true,
                    Scope = "Academic",
                    Group = "Academic",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }


            );

        }

    }
}