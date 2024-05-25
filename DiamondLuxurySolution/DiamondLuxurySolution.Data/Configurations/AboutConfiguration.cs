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
    public class AboutConfiguration : IEntityTypeConfiguration<About>
    {
        public void Configure(EntityTypeBuilder<About> builder)
        {
            builder.ToTable("Abouts");

            builder.HasKey(a => a.AboutId);
            builder.Property(a => a.AboutId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(a => a.AboutName).IsRequired().HasMaxLength(250);
            builder.Property(a => a.Description).HasMaxLength(250);
            builder.Property(a => a.AboutImage);
        }
    }

}
