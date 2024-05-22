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
    public class SlideConfiguration : IEntityTypeConfiguration<Slide>
    {
        public void Configure(EntityTypeBuilder<Slide> builder)
        {
            builder.ToTable("Slides");

            builder.HasKey(s => s.SlideId);
            builder.Property(s => s.SlideName).IsRequired().HasMaxLength(250);
            builder.Property(s => s.Description);
            builder.Property(s => s.SlideUrl).IsRequired();
            builder.Property(s => s.SlideImage).IsRequired();
            builder.Property(s => s.Status).IsRequired();
        }
    }

}
