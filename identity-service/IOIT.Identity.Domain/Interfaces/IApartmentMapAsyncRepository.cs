using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.ViewModels.PagingQuery;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IApartmentMapAsyncRepository : IAsyncGenericRepository<ApartmentMap, Guid>
    {
        Task<List<ApartmentMap>> GetListByResidentAsync(long residentId, CancellationToken cancellationToken);
        Task<Apartment> GetApartmentNameAsync(long residentId, CancellationToken cancellationToken);
        Task<ApartmentMap> CheckResidentMainAsync(int? apartmentId, CancellationToken cancellationToken);
        Task<ApartmentMap> CheckApartmentResidentAsync(int? apartmentId, long? residentId, CancellationToken cancellationToken);
        Task<IPagedResult<ApartmentDT>> GetByPageApartmentAsync(FilterPagination paging, long? ResidentId, CancellationToken cancellationToken);
        Task<IPagedResult<ResidentDT>> GetByPageMemberApartmentAsync(FilterPagination paging, int? ApartmentId, CancellationToken cancellationToken);
        Task<IPagedResult<ResidentDT>> GetMemberApartmentAdminAsync(FilterPagination paging, int? ApartmentId, byte? type, CancellationToken cancellationToken);
        Task<ResGetCountApartmentWithTypeResident> GetCountApartmentWithTypeResidentAsync(int? projectId, int projectTokenId, int userType, DateTime? dateStart, DateTime? dateEnd);
    }
}
