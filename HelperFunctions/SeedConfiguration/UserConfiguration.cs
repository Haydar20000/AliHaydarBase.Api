using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AliHaydarBase.Api.HelperFunctions.SeedConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {
                Id = "09cb0259-1714-4576-9a22-fca5b700817e",
                Email = "hay19732000@gmail.com",
                NormalizedEmail = "HAY19732000@GMAIL.COM",
                UserName = "hay19732000@gmail.com",
                NormalizedUserName = "HAY19732000@GMAIL.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEBHVJRSiK294d/4cDFS9xZ7GGNdlMo4Zqghw+JpfbTdRP4z2YlMflon/4iSVeZT//w==",
                FirstName = "HAYDER",
                LastName = "ALZEYAD",
                EmailConfirmed = true,
                SecurityStamp = "UABF5YCL37DBZSJNMXTOXZ4QMQ3BEQRS",
                ConcurrencyStamp = "09cb0259-1714-4576-9a22-fca5b700817e",
                LockoutEnabled = false,
            });
        }
    }
}