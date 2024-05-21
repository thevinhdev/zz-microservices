using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class PositionConfiguration : AppEntityTypeConfiguration<Position>
    {
        public override void Configure(EntityTypeBuilder<Position> builder)
        {
            base.Configure(builder);
            builder.ToTable("Position");

            builder.Property(c => c.Name).HasMaxLength(500).IsRequired();
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired(false);
            builder.Property(c => c.ProjectId).IsRequired(false);
            builder.Property(c => c.TowerId).IsRequired(false);
            builder.Property(c => c.LevelId).IsRequired(false);
            builder.Property(c => c.Note).HasMaxLength(2000).IsRequired(false);
        }
    }
}
