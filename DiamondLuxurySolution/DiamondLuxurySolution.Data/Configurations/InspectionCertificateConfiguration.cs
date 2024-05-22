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
    public class InspectionCertificateConfiguration : IEntityTypeConfiguration<InspectionCertificate>
    {
        public void Configure(EntityTypeBuilder<InspectionCertificate> builder)
        {
            builder.ToTable("InspectionCertificates");

            builder.HasKey(i => i.InspectionCertificateId);
            builder.Property(i => i.InspectionCertificateId).IsRequired().HasMaxLength(15);
            builder.Property(i => i.InspectionCertificateName).IsRequired().HasMaxLength(250);
            builder.Property(i => i.DateGrading).IsRequired();
            builder.Property(i => i.Logo).IsRequired();
        }
    }

}
