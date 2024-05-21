using IOIT.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IProjectAsyncRepository : IAsyncGenericRepository<Project, int>
    {
        Task<Project> FindByProjectIdAsync(int? projectId, CancellationToken cancellationToken);
        Task<Project> GetDataByCondition(string condition);
        Task<List<Project>> GetListDataByCondition(string condition);
    }
}
