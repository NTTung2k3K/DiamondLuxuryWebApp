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
    public class WarrantyDetailConfiguration : IEntityTypeConfiguration<WarrantyDetail>
    {

        public void Configure(EntityTypeBuilder<WarrantyDetail> builder)
        {
            builder.ToTable("WarrantyDetail");

            builder.HasKey(a => a.WarrantyDetailId);
            builder.Property(a => a.WarrantyDetailId).ValueGeneratedOnAdd();

            builder.Property(e => e.WarrantyDetailName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.WarrantyType).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ReceiveProductDate).IsRequired(false);
            builder.Property(e => e.ReturnProductDate).IsRequired(false);
            builder.Property(e => e.Description).IsRequired(false).HasMaxLength(500);
            builder.Property(e => e.Status).IsRequired(false).HasMaxLength(50);
            builder.Property(e => e.Image).IsRequired(false).HasMaxLength(int.MaxValue);

            builder.HasOne(e => e.Warranty)
                .WithMany(w => w.WarrantyDetails)
                .HasForeignKey(e => e.WarrantyId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
