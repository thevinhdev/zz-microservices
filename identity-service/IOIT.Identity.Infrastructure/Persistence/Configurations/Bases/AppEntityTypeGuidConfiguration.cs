using IOIT.Shared.Commons.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations.Bases
{
    public class AppEntityTypeGuidConfiguration<T> : IEntityTypeConfiguration<T> where T : AbstractEntity<Guid>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.CreatedAt);
            builder.Property(i => i.UpdatedAt);
            builder.Property(i => i.CreatedById);
            builder.Property(i => i.UpdatedById);
            builder.Property(i => i.Status).HasDefaultValue(EntityStatus.NORMAL);
        }
    }
}
