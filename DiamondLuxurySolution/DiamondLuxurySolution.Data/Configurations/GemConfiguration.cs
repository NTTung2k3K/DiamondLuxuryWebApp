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
    public class GemConfiguration : IEntityTypeConfiguration<Gem>
    {
        public void Configure(EntityTypeBuilder<Gem> builder)
        {
            builder.ToTable("Gems");

            builder.HasKey(g => g.GemId);
            builder.Property(c => c.GemName).IsRequired().HasMaxLength(250);
            builder.Property(g => g.ProportionImage);
            builder.Property(g => g.Symetry).HasMaxLength(250);
            builder.Property(g => g.Polish).HasMaxLength(250);
            builder.Property(g => g.IsOrigin).IsRequired();
<<<<<<< HEAD
            builder.Property(g => g.GemImage).HasMaxLength(int.MaxValue);
=======
            builder.Property(g => g.GemImage);
>>>>>>> 82106bed7b2dca6b6a9cc681d7adbedf5ad8dd6a
            builder.Property(g => g.Fluoresence).IsRequired();
            builder.Property(g => g.Active).IsRequired();
        }
    }

}
