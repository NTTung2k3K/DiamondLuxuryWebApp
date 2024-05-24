using DiamondLuxurySolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Configurations
{
    public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.ToTable("Platforms");

            builder.HasKey(p => p.PlatformId);
            builder.Property(p => p.PlatformName).IsRequired().HasMaxLength(250);
            builder.Property(p => p.PlatformUrl).IsRequired();
            builder.Property(p => p.PlatformLogo).IsRequired();
            builder.Property(g => g.Status).IsRequired();
        }
    }

}
