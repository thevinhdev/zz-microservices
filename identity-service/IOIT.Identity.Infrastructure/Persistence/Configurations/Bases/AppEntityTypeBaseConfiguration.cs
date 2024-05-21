using IOIT.Shared.Commons.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations.Bases
{
    public class AppEntityTypeBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity<Guid>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.CreatedAt);
            builder.Property(i => i.UpdatedAt);
        }
    }
}
