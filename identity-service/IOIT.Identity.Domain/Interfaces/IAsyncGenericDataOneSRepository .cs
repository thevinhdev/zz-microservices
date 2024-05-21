using IOIT.Shared.Commons.BaseEntities;
using IOIT.Shared.ViewModels.DatabaseOneS;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IAsyncGenericDataOneSRepository<TEntity>
        where TEntity : Temp_Base
    {
        List<TEntity> All();
    }
}
