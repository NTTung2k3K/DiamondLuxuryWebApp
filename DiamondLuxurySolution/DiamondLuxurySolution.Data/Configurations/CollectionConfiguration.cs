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
    public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
    {
        public void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.ToTable("Collections");

            builder.HasKey(c => c.CollectionId);
            builder.Property(c => c.CollectionId).IsRequired().HasMaxLength(30);
            builder.Property(c => c.CollectionName).HasMaxLength(250);
            builder.Property(c => c.Description).HasMaxLength(250);
            builder.Property(c => c.Thumbnail);
        }
    }

}
