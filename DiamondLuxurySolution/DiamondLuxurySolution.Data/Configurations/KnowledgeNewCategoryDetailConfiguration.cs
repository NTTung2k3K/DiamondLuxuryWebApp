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
    public class KnowledgeNewCategoryDetailConfiguration : IEntityTypeConfiguration<KnowledgeNewCatagoriesDetail>
    {
        public void Configure(EntityTypeBuilder<KnowledgeNewCatagoriesDetail> builder)
        {
            builder.ToTable("KnowledgeNewCatagoriesDetail");

            builder.HasKey(k => new { k.KnowledgeNewsId, k.KnowledgeNewCatagoryId });
            builder.Property(k => k.Description).HasMaxLength(int.MaxValue);

            builder.HasOne(k => k.KnowledgeNews)
                   .WithMany(k => k.KnowledgeNewCatagoriesDetails)
                   .HasForeignKey(k => k.KnowledgeNewsId)
                   .IsRequired();

            builder.HasOne(k => k.KnowledgeNewCatagory)
                   .WithMany(k => k.KnowledgeNewCatagoriesDetails)
                   .HasForeignKey(k => k.KnowledgeNewCatagoryId)
                   .IsRequired();
        }
    }

}
