using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : AppEntityTypeLongConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.ToTable("User");

            builder.Property(c => c.FullName).HasMaxLength(255).IsRequired();
            builder.Property(c => c.UserName).HasMaxLength(255).IsRequired(false);
            builder.Property(c => c.Password).HasMaxLength(200).IsRequired(false);
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired(false);
            builder.Property(c => c.Avata).HasColumnType("ntext").IsRequired(false);
            builder.Property(c => c.PositionId).IsRequired(false);
            builder.Property(c => c.DepartmentId).IsRequired(false);
            builder.Property(c => c.CardId).HasMaxLength(50).IsRequired(false);
            builder.Property(c => c.Phone).HasMaxLength(50).IsRequired(false).HasDefaultValue(string.Empty);
            builder.Property(c => c.Email).HasMaxLength(1000).IsRequired(false);
            builder.Property(c => c.Note).IsRequired(false);
            builder.Property(c => c.Address).HasMaxLength(500).IsRequired(false);
            builder.Property(c => c.KeyRandom).HasMaxLength(20).IsRequired(false);
            builder.Property(c => c.TypeThird).IsRequired(false);
            builder.Property(c => c.UserMapId).IsRequired(false);
            builder.Property(c => c.Type).IsRequired(false);
            builder.Property(c => c.LastLoginAt).IsRequired(false);
            builder.Property(c => c.RegEmail).HasMaxLength(50).IsRequired(false);
            builder.Property(c => c.RoleMax).IsRequired(false);
            builder.Property(c => c.RoleLevel).IsRequired(false);
            builder.Property(c => c.IsRoleGroup).IsRequired(false);
            builder.Property(c => c.IsPhoneConfirm).IsRequired(false);
            builder.Property(c => c.IsEmailConfirm).IsRequired(false);
            builder.Property(c => c.RegisterCode).HasMaxLength(10).IsRequired(false);
            builder.Property(c => c.CountLogin).IsRequired(false);
            builder.Property(c => c.LanguageId).IsRequired(false);
            builder.Property(c => c.ProjectId).IsRequired(false);
            builder.Property(c => c.IsDeletedByGuest).IsRequired(false);
        }
    }
}
