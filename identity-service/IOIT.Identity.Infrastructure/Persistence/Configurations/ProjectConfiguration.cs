using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : AppEntityTypeConfiguration<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            base.Configure(builder);
            builder.ToTable("Project");

            builder.Property(c => c.ProjectId).IsRequired();
            builder.Property(c => c.OneSId).IsRequired(false);
            builder.Property(c => c.Name).HasMaxLength(500).IsRequired();
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired(false);
        }
    }
}
