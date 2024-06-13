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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");

            builder.HasMany(u => u.CustomerOrders)
                  .WithOne(o => o.Customer)
                  .HasForeignKey(o => o.CustomerId);

            builder.HasMany(u => u.StaffOrders)
                  .WithOne(o => o.Staff)
                  .HasForeignKey(o => o.StaffId);

            builder.HasMany(u => u.ShipperOrders)
                  .WithOne(o => o.Shipper)
                  .HasForeignKey(o => o.ShipperId);
        }
    }
}
