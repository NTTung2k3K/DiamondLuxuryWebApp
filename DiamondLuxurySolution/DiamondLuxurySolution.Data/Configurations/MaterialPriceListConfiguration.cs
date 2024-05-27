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
    public class MaterialPriceListConfiguration : IEntityTypeConfiguration<MaterialPriceList>
    {
        public void Configure(EntityTypeBuilder<MaterialPriceList> builder)
        {
            builder.ToTable("MaterialPriceLists");

            builder.HasKey(m => m.MaterialPriceListId);
            builder.Property(m => m.BuyPrice).HasColumnType("decimal(10, 2)");
            builder.Property(m => m.SellPrice).HasColumnType("decimal(10, 2)");
            builder.HasOne(x => x.Material).WithMany(x => x.MaterialPriceLists).HasForeignKey(x => x.MaterialId).IsRequired();
            builder.Property(m => m.Active).IsRequired();
            builder.Property(m => m.effectDate).IsRequired();
        }
    }

}
