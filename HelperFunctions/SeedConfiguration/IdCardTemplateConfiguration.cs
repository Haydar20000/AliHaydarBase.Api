using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AliHaydarBase.Api.HelperFunctions.SeedConfiguration
{
    public class IdCardTemplateConfiguration : IEntityTypeConfiguration<IdCardTemplate>
    {
        public void Configure(EntityTypeBuilder<IdCardTemplate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.PrintHistories)
                   .WithOne(x => x.Template)
                   .HasForeignKey(x => x.TemplateId);

            builder.HasData(new IdCardTemplate
            {
                Id = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                Year = 2024,
                CompanyName = "Ali Haydar Co.",
                FrontImage = null,
                BackImage = null,
                FrontLayoutJson = "{}",
                BackLayoutJson = "{}",
                IsActive = true,
                TemplateName = "Default Template",
                CardWidthMm = 86,
                CardHeightMm = 54,
                Dpi = 300,
                CreatedAt = new DateTime(2024, 1, 1),
                UpdatedAt = new DateTime(2024, 1, 1)
            });
        }

    }

}