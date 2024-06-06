using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondLuxurySolution.Data.Entities;

namespace DiamondLuxurySolution.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");

            builder.HasKey(i => i.ImageId);
            builder.Property(i => i.Description);
            builder.Property(i => i.ImagePath).IsRequired();

            builder.HasOne(i => i.Product)
              .WithMany(p => p.Images)
              .HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }

}
