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


            builder.HasOne(cd => cd.Promotion)
                   .WithMany(x => x.CampaignDetails)
                   .HasForeignKey(cd => cd.PromotionId)
                   .IsRequired();

            builder.HasOne(cd => cd.Order)
                   .WithMany(x => x.CampaignDetails)
                   .HasForeignKey(cd => cd.OrderId)
                   .IsRequired();
        }
    }

}
