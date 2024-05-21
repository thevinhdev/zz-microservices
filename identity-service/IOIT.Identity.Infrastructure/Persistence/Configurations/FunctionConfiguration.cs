using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class FunctionConfiguration : AppEntityTypeConfiguration<Function>
    {
        public override void Configure(EntityTypeBuilder<Function> builder)
        {
            base.Configure(builder);
            builder.ToTable("Function");

            builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired(false);
            builder.Property(c => c.FunctionParentId).IsRequired(true);
            builder.Property(c => c.Url).HasMaxLength(200).IsRequired(false);
            builder.Property(c => c.Note).HasMaxLength(2000).IsRequired(false);
            builder.Property(c => c.Location).IsRequired(false);
            builder.Property(c => c.Icon).HasMaxLength(500).IsRequired(false);
            builder.Property(c => c.IsParamRoute).IsRequired(false);

        }
    }
}
