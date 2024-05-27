using DiamondLuxurySolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Configurations
{
    public class OrdersPaymentConfiguration : IEntityTypeConfiguration<OrdersPayment>
    {
        public void Configure(EntityTypeBuilder<OrdersPayment> builder)
        {
            builder.ToTable("OrdersPayments");

            builder.HasKey(x => x.OrdersPaymentId);
            builder.HasOne(x => x.Order).WithMany(x => x.OrdersPayment).HasForeignKey(x => x.OrderId);
            builder.HasOne(x => x.Payment).WithMany(x => x.OrdersPayment).HasForeignKey(x => x.PaymentId);
            builder.Property(x => x.Message).HasMaxLength(250);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.PaymentTime).IsRequired();

        }
    }
}
