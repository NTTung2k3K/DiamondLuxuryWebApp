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
    public class MaterialPriceListDetailConfiguration : IEntityTypeConfiguration<MaterialPriceListDetail>
    {
        public void Configure(EntityTypeBuilder<MaterialPriceListDetail> builder)
        {
            builder.ToTable("MaterialPriceListDetails");

            builder.HasKey(m => new { m.MaterialId, m.MaterialPriceListId });

            builder.Property(m => m.EffectDate).IsRequired();

            builder.HasOne(m => m.Material)
              .WithMany(m => m.MaterialPriceListDetails)
              .HasForeignKey(m => m.MaterialId);

            builder.HasOne(m => m.MaterialPriceList)
              .WithMany(m => m.MaterialPriceListDetails)
              .HasForeignKey(m => m.MaterialPriceListId);
        }
    }

}
