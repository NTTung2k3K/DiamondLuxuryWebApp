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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.OrderId);
            builder.Property(o => o.OrderId).HasMaxLength(20).IsRequired();
            builder.Property(o => o.ShipName).IsRequired().HasMaxLength(250);
            builder.Property(o => o.ShipPhoneNumber).IsRequired().HasMaxLength(250);
            builder.Property(o => o.ShipEmail).IsRequired().HasMaxLength(250);
            builder.Property(o => o.ShipAdress).HasMaxLength(250).IsRequired(false);
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.TotalAmout).HasColumnType("DECIMAL(15, 2)").IsRequired();
            builder.Property(o => o.Status).IsRequired();
            builder.HasOne(o => o.Customer)
                   .WithMany(x => x.CustomerOrders)
                   .HasForeignKey(o => o.CustomerId)
                   .IsRequired(false);
            builder.HasOne(o => o.Staff)
                   .WithMany(x => x.StaffOrders)
                   .HasForeignKey(o => o.StaffId)
                   .IsRequired(false);
            builder.HasOne(o => o.Shipper)
                   .WithMany(x => x.ShipperOrders)
                   .HasForeignKey(o => o.ShipperId)
                   .IsRequired(false);
            builder.HasOne(x => x.Discount).WithMany(x => x.Orders).HasForeignKey(x => x.DiscountId).IsRequired(false);
            builder.HasOne(x => x.Promotion).WithMany(x => x.Orders).HasForeignKey(o => o.PromotionId).IsRequired(false);
        }
    }

}
