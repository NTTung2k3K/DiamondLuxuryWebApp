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
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotions");

            builder.HasKey(p => p.PromotionId);
            builder.Property(p => p.PromotionId).IsRequired();
            builder.Property(p => p.PromotionName).HasMaxLength(250);
            builder.Property(p => p.Description).HasMaxLength(250);
            builder.Property(p => p.PromotionImage);
            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.EndDate).IsRequired();
            builder.Property(p => p.DiscountPercent);
            builder.Property(p => p.BannerImage);
            builder.Property(p => p.MaxDiscount);
        }
    }

}
