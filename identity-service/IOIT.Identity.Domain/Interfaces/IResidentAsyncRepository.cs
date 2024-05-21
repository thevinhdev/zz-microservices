using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.ViewModels.PagingQuery;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IResidentAsyncRepository : IAsyncGenericRepository<Resident, long>
    {
        Task<IPagedResult<ResResidentDT>> GetByPageAsync(FilterPagination paging, int? ProjectId, int? TowerId, int? FloorId, int? ApartmentId, int? type, CancellationToken cancellationToken);
        Task<Resident> CheckPhoneResidentMainAsync(string phoneMain, CancellationToken cancellationToken);
        Task<Resident> CheckPhoneExitAsync(string phoneMain, long residentId, CancellationToken cancellationToken);
        Task<Resident> CheckUserMapIdAsync(long? userMapId, CancellationToken cancellationToken);
        Task<Resident> CheckResidentExitAsync(int projectId, int towerId, int floorId, int apartmentId, CancellationToken cancellationToken);
        Task<Resident> CheckPhoneResidentGuestAsync(string phoneMain, int projectId, int towerId, int floorId, int apartmentId, CancellationToken cancellationToken);
        Task<IPagedResult<ResGetResidentByApartmentId>> GetByApartmentIdAndPageAsync(int? apartmentId, int? residentId, int pageSize, int pageIndex, CancellationToken cancellationToken);
        Task<IPagedResult<ResResidentIdDT>> GetApartmentResidentByPageAsync(FilterPagination paging, int? ProjectId, int? TowerId, int? FloorId, int? ApartmentId, CancellationToken cancellationToken);
        Task<int> GetCountResidentByProjectId(int projectId, CancellationToken cancellationToken);
        Task<ResGetResidentByNameAndPhoneInApartment?> GetResidentByNameOrPhoneInApartment(int apartmentId, string name, string phone, CancellationToken cancellationToken);
    }
}
