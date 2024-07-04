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
    public class KnowledgeNewCategoryConfiguration : IEntityTypeConfiguration<KnowledgeNewCatagory>
    {
        public void Configure(EntityTypeBuilder<KnowledgeNewCatagory> builder)
        {
            builder.ToTable("KnowledgeNewCategories");

            builder.HasKey(k => k.KnowledgeNewCatagoryId);
            builder.Property(k => k.KnowledgeNewCatagoryId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(k => k.KnowledgeNewCatagoriesName).HasMaxLength(250);
            builder.Property(k => k.Description).HasMaxLength(int.MaxValue);
        }
    }

}
