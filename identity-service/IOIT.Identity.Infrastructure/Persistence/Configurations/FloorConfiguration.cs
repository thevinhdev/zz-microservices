using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class FloorConfiguration : AppEntityTypeConfiguration<Floor>
    {
        public override void Configure(EntityTypeBuilder<Floor> builder)
        {
            base.Configure(builder);
            builder.ToTable("Floor");

            builder.Property(c => c.FloorId).IsRequired();
            builder.Property(c => c.TowerId).IsRequired(false);
            builder.Property(c => c.ProjectId).IsRequired(false);
            builder.Property(c => c.Name).HasMaxLength(500).IsRequired();
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired(false);
        }
    }
}
