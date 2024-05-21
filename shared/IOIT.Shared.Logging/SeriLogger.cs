using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace IOIT.Shared.Logging
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {
               var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
               var nameIndex = context.Configuration.GetValue<string>("ElasticConfiguration:IndexName");

               configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticUri))
                        {
                            ModifyConnectionSettings = x => x.BasicAuthentication("elastic", "TNG@2022"),
                            IndexFormat = $"applogs-{nameIndex?.ToLower()}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM-dd}",
                            AutoRegisterTemplate = true,
                            NumberOfShards = 2,
                            NumberOfReplicas = 1
                        })
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                    .ReadFrom.Configuration(context.Configuration);
                    //.CreateLogger();
           };
    }
}
