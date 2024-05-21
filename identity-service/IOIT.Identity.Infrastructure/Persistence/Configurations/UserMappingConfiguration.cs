using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class UserMappingConfiguration : AppEntityTypeConfiguration<UserMapping>
    {
        public override void Configure(EntityTypeBuilder<UserMapping> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserMapping");

            builder.Property(c => c.UserId).IsRequired(false);
            builder.Property(c => c.TargetId).IsRequired(false);
            builder.Property(c => c.TargetType).IsRequired(false);
        }
    }
}
