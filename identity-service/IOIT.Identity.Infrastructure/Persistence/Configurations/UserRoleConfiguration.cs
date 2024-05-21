using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class UserRoleConfiguration : AppEntityTypeConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserRole");

            builder.Property(c => c.UserId).IsRequired(true);
            builder.Property(c => c.RoleId).IsRequired(true);
        }
    }
}
