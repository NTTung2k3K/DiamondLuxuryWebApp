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
    public class WareHouseConfiguration : IEntityTypeConfiguration<WareHouse>
    {
        public void Configure(EntityTypeBuilder<WareHouse> builder)
        {
            builder.ToTable("WareHouses");

            builder.HasKey(w => w.WareHouseId);
            builder.Property(w => w.WareHouseName).IsRequired().HasMaxLength(250);
            builder.Property(w => w.Description);
            builder.Property(w => w.Location).IsRequired().HasMaxLength(250);
            builder.Property(w => w.AcquisitionDate).IsRequired();
        }
    }

}
