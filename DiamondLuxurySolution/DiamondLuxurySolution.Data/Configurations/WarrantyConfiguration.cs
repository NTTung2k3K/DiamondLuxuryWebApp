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
    public class WarrantyConfiguration : IEntityTypeConfiguration<DiamondLuxurySolution.Data.Entities.Warranty>
    {
        public void Configure(EntityTypeBuilder<DiamondLuxurySolution.Data.Entities.Warranty> builder)
        {
            builder.ToTable("Warrantys");

            builder.HasKey(w => w.WarrantyId);
            builder.Property(w => w.WarrantyId).IsRequired();
            builder.Property(w => w.WarrantyName).IsRequired().HasMaxLength(250);
            builder.Property(w => w.Description).HasMaxLength(250);
            builder.Property(w => w.DateActive).IsRequired();
            builder.Property(w => w.DateExpired).IsRequired();
        }
    }

}
