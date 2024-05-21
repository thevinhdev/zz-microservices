using IOIT.Identity.Application.Common.Interfaces;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Infrastructure.Persistence.Configurations.Bases;
using IOIT.Shared.Commons.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly ICurrentAdminService<Guid> _currentAdminService;
        public AppDbContext(DbContextOptions<AppDbContext> options,
            IDateTimeService datetime,
            ICurrentAdminService<Guid> currentAdminService) : base(options)
        {
            _dateTime = datetime;
            _currentAdminService = currentAdminService;
        }

        public virtual DbSet<Resident> Residents { get; set; }
        public virtual DbSet<ApartmentMap> ApartmentMaps { get; set; }
        public virtual DbSet<TypeAttributeItem> TypeAttributeItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var assembly = typeof(AppEntityTypeAdminConfiguration<>).Assembly;
            builder.ApplyConfigurationsFromAssembly(assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddAuditUserChange();

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            AddAuditUserChange();

            return base.SaveChanges();
        }

        private void AddAuditUserChange()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = _dateTime.Now;
                        entry.Entity.UpdatedAt = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = _dateTime.Now;
                        break;
                }
            }
        }
    }
}
