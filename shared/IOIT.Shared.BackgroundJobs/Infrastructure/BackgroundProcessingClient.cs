﻿using Hangfire;
using Hangfire.SqlServer;
using IOIT.Shared.BackgroundJobs.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IOIT.Shared.BackgroundJobs.Infrastructure
{
    public class BackgroundProcessingClient : IBackgroundProcessingClient
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<BackgroundJobSettings> _options;

        public BackgroundProcessingClient(IServiceProvider serviceProvider, IOptions<BackgroundJobSettings> options)
        {
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public string Enqueue(Expression<Action> action)
            => BackgroundJob.Enqueue(action);


        public void Run(Expression<Action> action)
        {
            try
            {
                action.Compile().Invoke();
            }
            catch
            {
                BackgroundJob.Enqueue(action);
            }
        }

        public async Task Run(Expression<Func<Task>> action)
        {
            try
            {
                await action.Compile().Invoke();
            }
            catch
            {
                BackgroundJob.Enqueue(action);
            }
        }

        public void RemoveRecurringJobIfExists(string recurringJobId)
            => RecurringJob.RemoveIfExists(recurringJobId);

        public void ConfigureRecurringJob<T>(
            string recurringJobId,
            string cronExpression,
            bool enabled = true)
            where T : IRecurringJob
        {
            var jobRunner = _serviceProvider.CreateScope().ServiceProvider.GetService<T>();

            var sqlStorage = new SqlServerStorage(_options.Value.ConnectionString);

            JobStorage.Current = sqlStorage;

            if (jobRunner == null)
                throw new Exception("Could not activate recurring job. Ensure it is configured on the service provider");

            if (enabled)
                RecurringJob.AddOrUpdate(
                    recurringJobId,
                    () => jobRunner.Run(JobCancellationToken.Null),
                    cronExpression,
                    TimeZoneInfo.FindSystemTimeZoneById(_options.Value.TimeZone) //local: SE Asia Standard Time    dev/prod: Asia/Ho_Chi_Minh
                );
            else
                RemoveRecurringJobIfExists(recurringJobId);
        }
    }
}
