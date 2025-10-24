using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AliHaydarBase.Api.HelperFunctions.SeedConfiguration
{
    public class RoleConfiguration: IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = "15bfe781-1f6a-465b-896e-6f233376b74d",
                    Name = "visitor",
                    NormalizedName = "VISITOR",
                    Description = "For Not subscribed User"
                },
                new Role
                {
                    Id = "22bfe781-1f6a-415b-896e-62233376b74d",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "For App Admin"
                }
            );
        }
    }
}