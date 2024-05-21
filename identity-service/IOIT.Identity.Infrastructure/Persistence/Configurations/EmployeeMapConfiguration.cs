using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class EmployeeMapConfiguration : AppEntityTypeConfiguration<EmployeeMap>
    {
        public override void Configure(EntityTypeBuilder<EmployeeMap> builder)
        {
            base.Configure(builder);
            builder.ToTable("EmployeeMap");

            builder.Property(c => c.EmployeeId).IsRequired(false);
            builder.Property(c => c.TowerId).IsRequired(false);
        }
    }
}
