using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Interfaces;
using IOIT.Identity.Application.Common.Interfaces.Cache;
using IOIT.Identity.Application.Common.Interfaces.Token;
using IOIT.Identity.Infrastructure.Caches;
//using IOIT.Identity.Infrastructure.Identity.Managers;
using IOIT.Identity.Infrastructure.Persistence;
using IOIT.Identity.Infrastructure.Persistence.Repositories;
using IOIT.Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IOIT.Identity.Application.Common.Interfaces.Options;
using IOIT.Identity.Application.Common.Interfaces.KeyMapCodeOption;

namespace IOIT.Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            services.AddScoped(typeof(IAsyncLongRepository<>), typeof(AsyncLongRepository<>));
            services.AddScoped(typeof(IAsyncGuidRepository<>), typeof(AsyncGuidRepository<>));
            services.AddTransient<IUserAsyncRepository, UserAsyncRepository>();
            services.AddTransient<IUserRoleAsyncRepository, UserRoleAsyncRepository>();
            services.AddTransient<IFunctionRoleAsyncRepository, FunctionRoleAsyncRepository>();
            services.AddTransient<IFunctionAsyncRepository, FunctionAsyncRepository>();
            services.AddTransient<IApartmentMapAsyncRepository, ApartmentMapAsyncRepository>();
            services.AddTransient<IProjectAsyncRepository, ProjectAsyncRepository>();
            services.AddTransient<IDepartmentAsyncRepository, DepartmentAsyncRepository>();
            services.AddTransient<ITowerAsyncRepository, TowerAsyncRepository>();
            services.AddTransient<IFloorAsyncRepository, FloorAsyncRepository>();
            services.AddTransient<IApartmentAsyncRepository, ApartmentAsyncRepository>();
            services.AddTransient<IResidentAsyncRepository, ResidentAsyncRepository>();
            services.AddTransient<IEmployeeAsyncRepository, EmployeeAsyncRepository>();
            services.AddTransient<IRoleAsyncRepository, RoleAsyncRepository>();
            services.AddTransient<ITypeAttributeItemAsyncRepository, TypeAttributeItemAsyncRepository>();
            services.AddTransient<IEmployeeMapAsyncRepository, EmployeeMapAsyncRepository>();
            services.AddTransient<IPositionAsyncRepository, PositionAsyncRepository>();
            services.AddTransient<IUserMappingAsyncRepository, UserMappingAsyncRepository>();
            services.AddTransient<IUtilitiesRepository, UtilitiesRepository>();
            services.AddTransient<IProjectUtilitiesRepository, ProjectUtilitiesRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var connnectionString = configuration.GetConnectionString(Constants.AppConnectionStringName);

            services.AddDbContext<AppDbContext>(options =>
                options.ConfigureWarnings(b => b.Log(CoreEventId.ManyServiceProvidersCreatedWarning))
                .UseSqlServer(connnectionString));

            services.Configure<CacheOptions>(configuration.GetSection("CacheOption"));
            services.Configure<JwtConfigOptions>(configuration.GetSection("JwtConfig"));
            services.Configure<AppsetingOption>(configuration.GetSection("AppSettings"));
            services.Configure<KeyMapCodeOption>(configuration.GetSection("KeyMapCode"));

            services.AddTransient<ICacheService, DistributedCacheService>();
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddSingleton<IDateTimeService, DateTimeService>();

            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
