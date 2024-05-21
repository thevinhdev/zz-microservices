using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class ResidentConfiguration : AppEntityTypeLongConfiguration<Resident>
    {
        public override void Configure(EntityTypeBuilder<Resident> builder)
        {
            base.Configure(builder);
            builder.ToTable("Resident");

            builder.Property(c => c.OneSid).IsRequired(false);
            builder.Property(c => c.FullName).HasMaxLength(500).IsRequired(false);
            builder.Property(c => c.Birthday).IsRequired(false);
            builder.Property(c => c.CardId).HasMaxLength(255).IsRequired(false);
            builder.Property(c => c.DateId).IsRequired(false);
            builder.Property(c => c.AddressId).HasMaxLength(500).IsRequired(false);
            builder.Property(c => c.Phone).HasMaxLength(50).IsRequired(false).HasDefaultValue(string.Empty);
            builder.Property(c => c.Email).HasMaxLength(500).IsRequired(false);
            builder.Property(c => c.Address).HasColumnType("ntext").IsRequired(false);
            builder.Property(c => c.Avata).HasColumnType("ntext").IsRequired(false);
            builder.Property(c => c.Sex).HasMaxLength(50).IsRequired(false);
            builder.Property(c => c.Note).HasColumnType("ntext").IsRequired(false);
            builder.Property(c => c.DateRent).IsRequired(false);
            builder.Property(c => c.Type).IsRequired(false);
            builder.Property(c => c.TypeCardId).IsRequired(false);
            builder.Property(c => c.CountryId).IsRequired(false);
        }
    }
}
