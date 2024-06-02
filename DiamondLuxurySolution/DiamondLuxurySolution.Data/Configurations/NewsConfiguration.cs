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
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.ToTable("News");

            builder.HasKey(n => n.NewsId);
            builder.Property(n => n.NewsId).IsRequired();
            builder.Property(n => n.NewName).HasMaxLength(250);
            builder.Property(n => n.Title).HasMaxLength(250);
            builder.Property(n => n.Image).HasMaxLength(int.MaxValue);
            builder.Property(n => n.Description).HasMaxLength(int.MaxValue);
            builder.Property(n => n.DateCreated);
            builder.Property(n => n.DateModified);

            builder.HasOne(n => n.Writer)
                   .WithMany(x => x.News)
                   .HasForeignKey(n => n.Id)
                   .IsRequired();
        }
    }

}
