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
    public class SubGemDetailConfiguration : IEntityTypeConfiguration<SubGemDetail>
    {
        public void Configure(EntityTypeBuilder<SubGemDetail> builder)
        {
            builder.ToTable("SubGemDetails");

            builder.HasKey(x => new {x.ProductId,x.SubGemId} );
            builder.HasOne(x => x.Product).WithMany(x => x.SubGemDetails).HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.SubGem).WithMany(x => x.SubGemDetails).HasForeignKey(x => x.SubGemId);


        }
    }
}
