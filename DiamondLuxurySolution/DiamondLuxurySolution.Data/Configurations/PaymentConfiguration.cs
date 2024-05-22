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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.PaymentId);
            builder.Property(p => p.PaymentId).IsRequired();
            builder.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Description).HasMaxLength(250);
            builder.Property(p => p.Message).HasMaxLength(250);
            builder.Property(p => p.PaymentTime).IsRequired();
            builder.Property(p => p.Status).IsRequired();

            builder.HasOne(p => p.Order)
                   .WithMany()
                   .HasForeignKey(p => p.OrderId)
                   .IsRequired();
        }
    }

}
