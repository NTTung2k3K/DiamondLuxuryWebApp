using DiamondLuxurySolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Configurations
{
    internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contact");
            builder.HasKey(c => c.ContactId);
            builder.Property(c => c.ContactId).ValueGeneratedOnAdd();
            builder.Property(c => c.ContactPhoneUser).HasMaxLength(250);
            builder.Property(c => c.ContactNameUser).HasMaxLength(250);
            builder.Property(c => c.Content);
            builder.Property(c => c.ContactEmailUser).HasMaxLength(250);
            builder.Property(c => c.IsResponse);
        }
    }
}
