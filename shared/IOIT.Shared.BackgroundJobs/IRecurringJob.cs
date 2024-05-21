using Hangfire;
using System.Threading.Tasks;

namespace IOIT.Shared.BackgroundJobs
{
    public interface IRecurringJob
    {
        Task<Task> Run(IJobCancellationToken cancellationToken);
    }
}
