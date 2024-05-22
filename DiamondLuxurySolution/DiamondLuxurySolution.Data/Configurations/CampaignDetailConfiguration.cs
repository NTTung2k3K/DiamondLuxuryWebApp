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
    public class CampaignDetailConfiguration : IEntityTypeConfiguration<CampaignDetail>
    {
        public void Configure(EntityTypeBuilder<CampaignDetail> builder)
        {
            builder.ToTable("CampaignDetail");

            builder.HasKey(cd => new { cd.OrderId, cd.PromotionId });

            builder.Property(cd => cd.DiscountPercentage).IsRequired();
            builder.Property(cd => cd.FromAmount).HasColumnType("DECIMAL(10, 2)").IsRequired();
            builder.Property(cd => cd.ToAmount).HasColumnType("DECIMAL(10, 2)").IsRequired();
            builder.Property(cd => cd.MaxDiscount).HasColumnType("DECIMAL(10, 2)").IsRequired();

            builder.HasOne(cd => cd.Promotion)
                   .WithMany()
                   .HasForeignKey(cd => cd.PromotionId)
                   .IsRequired();

            builder.HasOne(cd => cd.Order)
                   .WithMany()
                   .HasForeignKey(cd => cd.OrderId)
                   .IsRequired();
        }
    }

}
