using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Configurations
{
        public class SubGemConfiguration : IEntityTypeConfiguration<DiamondLuxurySolution.Data.Entities.SubGem>
        {
            public void Configure(EntityTypeBuilder<DiamondLuxurySolution.Data.Entities.SubGem> builder)
            {
                builder.ToTable("SubGems");

                builder.HasKey(w => w.SubGemId);

            }
        }
}
