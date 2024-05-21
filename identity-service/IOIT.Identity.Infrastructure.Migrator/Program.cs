using IOIT.Identity.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace IOIT.Identity.Infrastructure.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("Hello World!");
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            var config = LoadConfiguration();

            services.AddSingleton(config);
            services.AddLogging();
            services.RegisterInfrastructureServices(config);

            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            var path = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
