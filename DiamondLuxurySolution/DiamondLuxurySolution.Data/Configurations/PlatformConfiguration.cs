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
            builder.Property(p => p.PlatformName).HasMaxLength(250);
            builder.Property(p => p.PlatformUrl).HasMaxLength(int.MaxValue);
            builder.Property(p => p.PlatformLogo);
        }
    }

}
