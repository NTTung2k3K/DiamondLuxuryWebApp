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
                           .WithMany(g => g.ProductsGems) 
                           .HasForeignKey(pg => pg.GemId)
                           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pg => pg.Product)
                   .WithMany(p => p.ProductsGems) 
                   .HasForeignKey(pg => pg.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(pg => pg.MainGemQuantity);
            builder.Property(pg => pg.SubGemQuantity);


           

        }
    }


}
