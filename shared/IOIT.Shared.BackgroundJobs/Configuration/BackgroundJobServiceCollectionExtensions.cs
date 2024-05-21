using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using IOIT.Shared.BackgroundJobs.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.BackgroundJobs.Configuration
{
    public static class BackgroundJobServiceCollectionExtensions
    {
        public static IServiceCollection AddHangfireBackgroundJobServer(this IServiceCollection services,
            IConfiguration config)
        {
            var settings = new BackgroundJobSettings();
            config.Bind(settings);

            services
                .Configure<BackgroundJobSettings>(config)
                ;

            services
                .AddTransient<IBackgroundProcessingClient, BackgroundProcessingClient>();

            if (!string.IsNullOrEmpty(settings.ConnectionString))
            {
                services
                    .AddHangfire(o => o
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSqlServerStorage(settings.ConnectionString, new SqlServerStorageOptions()
                        {
                            PrepareSchemaIfNecessary = false
                        })
                    );

                services.AddHangfireServer();
            }

            return services;
        }

        public static IApplicationBuilder UseBackgroundJobServerDashboard(this IApplicationBuilder app)
        {
            GlobalConfiguration.Configuration.UseActivator(new BackgroundProcessActivator(app.ApplicationServices));

            var settings = app.ApplicationServices.GetService<IOptionsMonitor<BackgroundJobSettings>>();

            if (!string.IsNullOrEmpty(settings.CurrentValue.ConnectionString))
                app
                    .UseHangfireServer(new BackgroundJobServerOptions
                    {
                        WorkerCount = 5
                    })
                    .UseHangfireDashboard("/hang", new DashboardOptions
                    {
                        Authorization = new List<IDashboardAuthorizationFilter>(),
                        IgnoreAntiforgeryToken = true
                    });

            return app;
        }
    }
}
