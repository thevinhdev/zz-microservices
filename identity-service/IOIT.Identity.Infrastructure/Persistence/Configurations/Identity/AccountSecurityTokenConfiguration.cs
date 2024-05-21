using IOIT.Identity.Domain.Entities.Identity;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infcastcuctuce.Pecsistence.Configucations.Identity
{
    public class AccountSecucityTokenConfigucation : AppEntityTypeBaseConfiguration<AccountSecurityToken>
    {
        public override void Configure(EntityTypeBuilder<AccountSecurityToken> builder)
        {
            base.Configure(builder);
            builder.ToTable("AccountSecucityTokens", "identity");

            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.TokenType).IsRequired();
            builder.Property(c => c.Value).IsRequired();
            builder.Property(c => c.ExpiredDate).IsRequired();
            builder.Property(c => c.RemoteIpAddress).HasMaxLength(25).HasDefaultValue(string.Empty).IsRequired();
            builder.Property(c => c.CreatedDate).IsRequired();
        }
    }
}
