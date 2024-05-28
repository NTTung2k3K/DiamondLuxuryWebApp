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
    public class FrameConfiguration : IEntityTypeConfiguration<Frame>
    {
        public void Configure(EntityTypeBuilder<Frame> builder)
        {
            builder.ToTable("Frames");

            builder.HasKey(g => g.FrameId);

            builder.HasOne(g => g.Material).WithMany(x => x.Frames).HasForeignKey(x => x.MaterialId).IsRequired();
        }


    }
}
