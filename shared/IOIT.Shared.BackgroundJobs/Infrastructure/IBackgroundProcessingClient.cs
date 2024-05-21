using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IOIT.Shared.BackgroundJobs.Infrastructure
{
    public interface IBackgroundProcessingClient
    {
        string Enqueue(Expression<Action> action);
        void Run(Expression<Action> action);
        Task Run(Expression<Func<Task>> action);
        void RemoveRecurringJobIfExists(string recurringJobId);

        void ConfigureRecurringJob<T>(
            string recurringJobId,
            string cronExpression,
            bool enabled = true)
            where T : IRecurringJob;
    }
}
