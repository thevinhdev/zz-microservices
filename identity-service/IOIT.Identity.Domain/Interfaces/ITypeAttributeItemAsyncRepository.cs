using IOIT.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface ITypeAttributeItemAsyncRepository : IAsyncGenericRepository<TypeAttributeItem, int>
    {
        Task<TypeAttributeItem> FindByTypeAttributeItemIdAsync(int? typeAttributeItemId, CancellationToken cancellationToken);
        Task<TypeAttributeItem> GetDataByCondition(string condition);
        Task<List<TypeAttributeItem>> GetListDataByCondition(string condition);
    }
}
