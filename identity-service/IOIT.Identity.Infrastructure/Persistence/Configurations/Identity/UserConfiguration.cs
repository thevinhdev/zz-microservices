using IOIT.Identity.Domain.Entities.Indentity;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations.Indentity
{
    public class UserConfiguration : AppEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.ToTable("Users", "identity");

            builder.Property(c => c.UserName).HasMaxLength(255).IsRequired();
            builder.Property(c => c.FullName).HasMaxLength(255).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
            builder.Property(c => c.Phone).HasMaxLength(50).HasDefaultValue(string.Empty);
            builder.Property(c => c.Password).HasMaxLength(255);
            builder.Property(c => c.Address).HasMaxLength(1024).HasDefaultValue(string.Empty);
            builder.Property(c => c.Code).HasMaxLength(10).HasDefaultValue(string.Empty);
            builder.Property(c => c.Note).HasMaxLength(1024);
            builder.Property(c => c.Avata).HasMaxLength(1024);
            //builder.Property(c => c.IsActive).HasDefaultValue(true);
        }
    }
}
