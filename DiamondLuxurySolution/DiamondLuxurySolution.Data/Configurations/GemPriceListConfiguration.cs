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
    public class GemPriceListConfiguration : IEntityTypeConfiguration<GemPriceList>
    {
        public void Configure(EntityTypeBuilder<GemPriceList> builder)
        {
            builder.ToTable("GemPriceLists");

            builder.HasKey(g => g.GemPriceListId);
            builder.Property(g => g.Cut).HasMaxLength(30);
            builder.Property(g => g.Clarity).HasMaxLength(10);
            builder.Property(g => g.CaratWeight).HasMaxLength(50);
            builder.Property(g => g.Color).HasMaxLength(10);
            builder.Property(g => g.Price).HasColumnType("decimal(15, 2)");
            builder.HasOne(x => x.Gem).WithMany(x => x.GemPriceLists).HasForeignKey(x => x.GemId).IsRequired();
            builder.Property(g => g.Active).IsRequired();
            builder.Property(g => g.effectDate).IsRequired();
        }
    }

}
