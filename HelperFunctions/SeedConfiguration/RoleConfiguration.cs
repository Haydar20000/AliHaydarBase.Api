using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AliHaydarBase.Api.HelperFunctions.SeedConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = SeedConstants.VisitorRoleId,
                    Name = "visitor",
                    NormalizedName = "VISITOR",
                    Description = "For Not subscribed User"
                },
                new Role
                {
                    Id = SeedConstants.AdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "For App Admin"
                }
            );
        }
    }
}