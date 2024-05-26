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
=======
            builder.Property(g => g.GemImage);
>>>>>>> 59348e02b106350021dbba36fe0bb84fc3d839e4
            builder.Property(g => g.Fluoresence).IsRequired();
            builder.Property(g => g.Active).IsRequired();
        }
    }

}
