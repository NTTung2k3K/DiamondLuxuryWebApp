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
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("Materials");

            builder.HasKey(m => m.MaterialId);
            builder.Property(m => m.MaterialName).HasMaxLength(250);
            builder.Property(m => m.Description);
            builder.Property(m => m.Color).HasMaxLength(250);
            builder.Property(m => m.Weight);
            builder.Property(m => m.Price);
            builder.Property(m => m.EffectDate);
            builder.Property(m => m.MaterialImage);
        }
    }

}
