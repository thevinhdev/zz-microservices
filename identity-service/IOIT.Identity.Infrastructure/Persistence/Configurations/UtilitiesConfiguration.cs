using System;
using System.Collections.Generic;
using System.Text;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Enum;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class UtilitiesConfiguration : AppEntityTypeConfiguration<Domain.Entities.Utilities>
    {
        public override void Configure(EntityTypeBuilder<Domain.Entities.Utilities> builder)
        {
            base.Configure(builder);
            builder.ToTable("Utilities");
            
            builder.Property(c => c.Name).HasMaxLength(255).IsRequired();
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Type).HasDefaultValue(DomainEnum.TypeUtilities.Utilities).IsRequired();
            builder.Property(c => c.Url).HasMaxLength(512).HasDefaultValue(string.Empty).IsRequired();
            builder.Property(c => c.Icon).HasMaxLength(512).HasDefaultValue(string.Empty).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(1024).HasDefaultValue(string.Empty).IsRequired();
            builder.Property(c => c.Order).HasDefaultValue(0).IsRequired();
        }
    }
}
