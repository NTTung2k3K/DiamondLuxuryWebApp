using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.ToTable("AppUsersRoles");
            //builder.HasKey(x => new { x.UserId, x.RoleId });
            builder.HasOne(x => x.User).WithMany(x => x.AppUserRoles).HasForeignKey(x => x.UserId).IsRequired();
            builder.HasOne(x => x.Role).WithMany(x => x.AppUserRoles).HasForeignKey(x => x.RoleId).IsRequired();
        }
    }
}
