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
    public class ProductGemConfiguration : IEntityTypeConfiguration<ProductsGem>
    {
        public void Configure(EntityTypeBuilder<ProductsGem> builder)
        {
            builder.ToTable("Products_Gems");

            builder.HasKey(pg => new { pg.GemId, pg.ProductId });

            builder.HasOne(pg => pg.Gem)
                   .WithMany()
                   .HasForeignKey(pg => pg.GemId);

            builder.HasOne(pg => pg.Product)
                   .WithMany()
                   .HasForeignKey(pg => pg.ProductId);

            builder.Property(pg => pg.MainGemPrice).IsRequired();
            builder.Property(pg => pg.SubGemPrice).IsRequired();
        }
    }


}
