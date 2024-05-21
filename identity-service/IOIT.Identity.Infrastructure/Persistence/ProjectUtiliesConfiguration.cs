using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;

namespace IOIT.Identity.Infrastructure.Persistence
{
    public class ProjectUtiliesConfiguration : AppEntityTypeConfiguration<Domain.Entities.ProjectUtilities>
    {
        public override void Configure(EntityTypeBuilder<Domain.Entities.ProjectUtilities> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProjectUtilities");

            builder.Property(c => c.ProjectId).IsRequired();
            builder.Property(c => c.UtilitiesId).IsRequired();
            builder.Property(c => c.IsActive).HasDefaultValue(true).IsRequired();
            builder.Property(c => c.Note).HasDefaultValue(string.Empty).IsRequired();
            builder.Property(c => c.ActivedDate);
            builder.Property(c => c.ExpiredDate);
            builder.Property(c => c.CreatedBy).HasDefaultValue(string.Empty).IsRequired();
            builder.Property(c => c.UpdatedBy).HasDefaultValue(string.Empty).IsRequired();
        }
    }
}
