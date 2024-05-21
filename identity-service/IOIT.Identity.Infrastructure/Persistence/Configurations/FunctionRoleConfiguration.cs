using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class FunctionRoleConfiguration : AppEntityTypeConfiguration<FunctionRole>
    {
        public override void Configure(EntityTypeBuilder<FunctionRole> builder)
        {
            base.Configure(builder);
            builder.ToTable("FunctionRole");

            builder.Property(c => c.TargetId).IsRequired(true);
            builder.Property(c => c.FunctionId).IsRequired(true);
            builder.Property(c => c.ActiveKey).HasMaxLength(20).IsRequired(false);
            builder.Property(c => c.Type).IsRequired(false);

        }
    }
}
