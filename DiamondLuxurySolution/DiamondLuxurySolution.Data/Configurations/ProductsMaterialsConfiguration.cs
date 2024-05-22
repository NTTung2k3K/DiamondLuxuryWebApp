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
    public class ProductMaterialConfiguration : IEntityTypeConfiguration<ProductsMaterial>
    {
        public void Configure(EntityTypeBuilder<ProductsMaterial> builder)
        {
            builder.ToTable("Products_Materials");

            builder.HasKey(pm => new { pm.MaterialId, pm.ProductId });

            builder.HasOne(pm => pm.Material)
                   .WithMany()
                   .HasForeignKey(pm => pm.MaterialId);

            builder.HasOne(pm => pm.Product)
                   .WithMany()
                   .HasForeignKey(pm => pm.ProductId);

            builder.Property(pm => pm.Weight).IsRequired();
        }
    }


}
