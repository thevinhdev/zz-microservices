using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOIT.Identity.Infrastructure.Persistence.Configurations
{
    public class ApartmentMapConfiguration : AppEntityTypeGuidConfiguration<ApartmentMap>
    {
        public override void Configure(EntityTypeBuilder<ApartmentMap> builder)
        {
            base.Configure(builder);
            builder.ToTable("ApartmentMap");

            //builder.Property(c => c.Id).IsRequired(true);
            builder.Property(c => c.ApartmentId).IsRequired(false);
            builder.Property(c => c.FloorId).IsRequired(false);
            builder.Property(c => c.TowerId).IsRequired(false);
            builder.Property(c => c.ProjectId).IsRequired(false);
            builder.Property(c => c.ResidentId).IsRequired(false);
            builder.Property(c => c.RelationshipId).IsRequired(false);
            builder.Property(c => c.DateRent).IsRequired(false);
            builder.Property(c => c.DateStart).IsRequired(false);
            builder.Property(c => c.DateEnd).IsRequired(false);
            builder.Property(c => c.Type).IsRequired(false);
        }
    }
}
