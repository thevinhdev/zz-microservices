using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class TowerConfiguration : AppEntityTypeConfiguration<Tower>
    {
        public override void Configure(EntityTypeBuilder<Tower> builder)
        {
            base.Configure(builder);
            builder.ToTable("Tower");

            builder.Property(c => c.TowerId).IsRequired();
            builder.Property(c => c.ProjectId).IsRequired(false);
            builder.Property(c => c.Name).HasMaxLength(500).IsRequired();
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired(false);
        }
    }
}
