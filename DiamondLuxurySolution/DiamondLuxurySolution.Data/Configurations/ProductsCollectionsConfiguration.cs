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
    public class ProductCollectionConfiguration : IEntityTypeConfiguration<ProductsCollection>
    {
        public void Configure(EntityTypeBuilder<ProductsCollection> builder)
        {
            builder.ToTable("Products_Collections");

            builder.HasKey(pc => new { pc.CollectionId, pc.ProductId });

            builder.HasOne(pc => pc.Collection)
                   .WithMany()
                   .HasForeignKey(pc => pc.CollectionId);

            builder.HasOne(pc => pc.Product)
                   .WithMany()
                   .HasForeignKey(pc => pc.ProductId);

            builder.Property(pc => pc.Description).HasMaxLength(250);
        }
    }


}
