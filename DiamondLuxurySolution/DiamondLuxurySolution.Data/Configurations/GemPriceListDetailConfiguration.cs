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
    public class GemPriceListDetailConfiguration : IEntityTypeConfiguration<GemPriceListDetail>
    {
        public void Configure(EntityTypeBuilder<GemPriceListDetail> builder)
        {
            builder.ToTable("GemPriceListDetails");

            builder.HasKey(g => new { g.GemId, g.GemPriceListId });
            builder.Property(g => g.EffectDate).IsRequired();

            builder.HasOne(g => g.Gem)
              .WithMany(g => g.GemPriceListDetails)
              .HasForeignKey(g => g.GemId);

            builder.HasOne(g => g.GemPriceList)
              .WithMany(g => g.GemPriceListDetails)
              .HasForeignKey(g => g.GemPriceListId);
        }
    }

}
