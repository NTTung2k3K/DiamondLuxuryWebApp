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
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discounts");

            builder.HasKey(d => d.DiscountId);
            builder.Property(d => d.DiscountId).IsRequired();
            builder.Property(d => d.DiscountName).IsRequired().HasMaxLength(250);
            builder.Property(d => d.Description).HasMaxLength(250);
            builder.Property(d => d.DiscountImage).IsRequired().HasMaxLength(250);
            builder.Property(d => d.PercentSale).IsRequired();
            builder.Property(d => d.StartDate).IsRequired();
            builder.Property(d => d.EndDate).IsRequired();
        }
    }

}
