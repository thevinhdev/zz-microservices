using IOIT.Identity.Application.Common;
using IOIT.Identity.Infrastructure.Persistence;
using IOIT.Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace IOIT.Identity.Infrastructure.Migrator.DesignTimeDbContextFactory
{
    public class AppContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var migrationsAssembly = typeof(Program).Assembly.FullName;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = configuration.GetConnectionString(Constants.AppConnectionStringName);

            builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));

            var dateTimeService = new DateTimeService();
            var currentAdminService = new StuffCurrentAdminService<Guid>();

            return new AppDbContext(builder.Options, dateTimeService, currentAdminService);
        }
    }
}
