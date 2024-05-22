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
            builder.Property(g => g.Cut).IsRequired().HasMaxLength(30);
            builder.Property(g => g.Clarity).IsRequired().HasMaxLength(10);
            builder.Property(g => g.CaratWeight).IsRequired().HasMaxLength(10);
            builder.Property(g => g.Color).IsRequired().HasMaxLength(10);
            builder.Property(g => g.Price).IsRequired().HasColumnType("decimal(10, 2)");
        }
    }

}
