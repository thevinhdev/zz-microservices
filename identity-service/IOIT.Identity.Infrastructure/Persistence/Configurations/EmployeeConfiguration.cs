using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class EmployeeConfiguration : AppEntityTypeConfiguration<Employee>
    {
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            base.Configure(builder);
            builder.ToTable("Employee");

            builder.Property(c => c.FullName).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired(false);
            builder.Property(c => c.Avata).HasMaxLength(1024).IsRequired(false);
            builder.Property(c => c.ProjectId).IsRequired(false);
            builder.Property(c => c.ProjectName).HasMaxLength(500).IsRequired(false);
            builder.Property(c => c.PositionId).IsRequired(false);
            builder.Property(c => c.DepartmentId).IsRequired(false);
            builder.Property(c => c.DepartmentName).HasMaxLength(200).IsRequired(false);
            builder.Property(c => c.Birthday).IsRequired(false);
            builder.Property(c => c.CardId).HasMaxLength(255).IsRequired(false);
            builder.Property(c => c.Phone).HasMaxLength(50).IsRequired(false).HasDefaultValue(string.Empty);
            builder.Property(c => c.Email).HasMaxLength(255).IsRequired(false);
            builder.Property(c => c.Address).HasMaxLength(1000).IsRequired(false);
            builder.Property(c => c.Note).HasMaxLength(2000).IsRequired(false);
            builder.Property(c => c.TypeEmployee).IsRequired(false);
            builder.Property(c => c.IsMain).IsRequired(false);
        }
    }
}
