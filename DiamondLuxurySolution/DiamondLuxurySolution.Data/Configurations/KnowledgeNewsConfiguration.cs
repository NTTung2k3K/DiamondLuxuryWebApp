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
    public class KnowledgeNewsConfiguration : IEntityTypeConfiguration<KnowledgeNews>
    {
        public void Configure(EntityTypeBuilder<KnowledgeNews> builder)
        {
            builder.ToTable("KnowledgeNews");

            builder.HasKey(k => k.KnowledgeNewsId);
            builder.Property(k => k.KnowledgeNewsId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(k => k.KnowledgeNewsName).IsRequired().HasMaxLength(250);
            builder.Property(k => k.Thumnail).HasMaxLength(int.MaxValue);
            builder.Property(k => k.Description).HasMaxLength(int.MaxValue);
            builder.Property(k => k.DateCreated);
            builder.Property(k => k.DateModified);

            builder.HasOne(k => k.Writer)
                   .WithMany(x => x.KnowledgeNews)
                   .HasForeignKey(k => k.WriterId)
                   .IsRequired();
        }
    }

}
