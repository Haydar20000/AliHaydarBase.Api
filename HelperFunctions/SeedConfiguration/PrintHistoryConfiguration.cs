using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AliHaydarBase.Api.HelperFunctions.SeedConfiguration
{
    public class PrintHistoryConfiguration : IEntityTypeConfiguration<PrintHistory>
    {
        public void Configure(EntityTypeBuilder<PrintHistory> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

}