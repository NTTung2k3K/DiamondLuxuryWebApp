using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondLuxurySolution.Data.Entities;

namespace DiamondLuxurySolution.Data.Configurations
{
    public class OrderDiscountConfiguration : IEntityTypeConfiguration<OrderDiscount>
    {
        public void Configure(EntityTypeBuilder<OrderDiscount> builder)
        {
            builder.ToTable("Orders_Discounts");

            builder.HasKey(od => new { od.OrderId, od.DiscountId });

            builder.HasOne(od => od.Discount)
                   .WithMany()
                   .HasForeignKey(od => od.DiscountId)
                   .IsRequired();

            builder.HasOne(od => od.Order)
                   .WithMany()
                   .HasForeignKey(od => od.OrderId)
                   .IsRequired();
        }
    }

}
