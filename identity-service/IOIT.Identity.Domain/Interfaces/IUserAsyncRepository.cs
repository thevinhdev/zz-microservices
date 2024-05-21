using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.ViewModels.PagingQuery;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IUserAsyncRepository : IAsyncGenericRepository<User, long>
    {
        Task<User> CheckUserLoginAsync(string userName, string password, CancellationToken cancellationToken);
        Task<User> CheckUserLoginAppAsync(string userName, string password, int type);
        Task<User> FindByEmailAsync(string email, long? userId, CancellationToken cancellationToken);
        Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken);
        Task<User> FindByResidentAsync(long residentId, CancellationToken cancellationToken);
        Task<User> FindByUsernameAsync(string userName, CancellationToken cancellationToken);
        Task<User> FindByPhoneUsernameAsync(string userName, int type, CancellationToken cancellationToken);
        Task<User> FindByUserMapIdAsync(long? userMapId, int type, CancellationToken cancellationToken);
        Task<User> FindByRegisterCode(long id, string registerCode, CancellationToken cancellationToken);
        Task<User> FindByUserCode(string userCode, CancellationToken cancellationToken);
        Task<User> CheckUserNameExitAsync(string userName, long? userId, CancellationToken cancellationToken);
        Task<IPagedResult<UserRoleDT>> GetByPageEmpAsync(FilterPagination paging, int? roleMax, int? roleLevel, int? projectId, CancellationToken cancellationToken);
    }
}
