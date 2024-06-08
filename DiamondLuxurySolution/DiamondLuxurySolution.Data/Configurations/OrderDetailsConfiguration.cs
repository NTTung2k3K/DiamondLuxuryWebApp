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
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");

            builder.HasKey(od => new { od.OrderId, od.ProductId, od.WarrantyId });

            builder.Property(od => od.Quantity).IsRequired();
            builder.Property(od => od.Discount).IsRequired();
            builder.Property(od => od.UnitPrice).HasColumnType("DECIMAL(15, 2)").IsRequired();
            builder.Property(od => od.TotalPrice).HasColumnType("DECIMAL(15, 2)").IsRequired();

            builder.HasOne(od => od.Product)
                   .WithMany(x => x.OrderDetails)
                   .HasForeignKey(od => od.ProductId)
                   .IsRequired();

            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId)
                   .IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(od => od.Warranty)
                   .WithMany(x => x.OrderDetails)
                   .HasForeignKey(od => od.WarrantyId)
                   .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
