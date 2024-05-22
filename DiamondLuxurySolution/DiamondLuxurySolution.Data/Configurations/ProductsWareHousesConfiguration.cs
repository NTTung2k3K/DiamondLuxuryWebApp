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
    public class ProductsWareHousesConfiguration : IEntityTypeConfiguration<ProductsWareHouse>
    {
        public void Configure(EntityTypeBuilder<ProductsWareHouse> builder)
        {
            builder.ToTable("ProductsWareHouses");

            builder.HasKey(pw => new { pw.WareHouseId, pw.ProductId });

            builder.HasOne(pw => pw.WareHouse)
              .WithMany(w => w.ProductsWareHouses) 
              .HasForeignKey(pw => pw.WareHouseId);

            builder.HasOne(pw => pw.Product)
              .WithMany(p => p.ProductsWareHouses)
              .HasForeignKey(pw => pw.ProductId);

            builder.Property(pw => pw.QuantityInStocks).IsRequired();
        }
    }


}
