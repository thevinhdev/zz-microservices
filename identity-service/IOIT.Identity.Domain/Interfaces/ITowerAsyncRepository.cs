using IOIT.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface ITowerAsyncRepository : IAsyncGenericRepository<Tower, int>
    {
        Task<Tower> FindByTowerIdAsync(int towerId, CancellationToken cancellationToken);
        Task<Tower> GetDataByCondition(string condition);
        Task<List<Tower>> GetListDataByCondition(string condition);
    }
}
