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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.CategoryId);
            builder.Property(c => c.CategoryName).HasMaxLength(250);
            builder.Property(c => c.CategoryType).HasMaxLength(250);
            builder.Property(c => c.CategoryImage);
            builder.Property(c => c.CategoryPriceProcessing);
        }
    }

}
