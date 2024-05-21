using AutoMapper;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Categorys.Queries
{
    public class GetByPageMemberApartmentQuery : FilterPagination, IRequest<IPagedResult<ResidentDT>>
    {
        public long? ResidentId { get; set; }
        public int? ApartmentId { get; set; }

        public class Handler : IRequestHandler<GetByPageMemberApartmentQuery, IPagedResult<ResidentDT>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApartmentMapAsyncRepository _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IApartmentMapAsyncRepository entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<IPagedResult<ResidentDT>> Handle(GetByPageMemberApartmentQuery request, CancellationToken cancellationToken)
            {
                //Kiểm tra xem người dùng và căn hộ có map với nhau không
                //ApartmentMap apartmentMap = db.ApartmentMap.Where(ap => ap.ApartmentId == ApartmentId && ap.ResidentId == ResidentId && ap.Status == (int)Const.Status.NORMAL).FirstOrDefault();
                ApartmentMap apartmentMap = await _entityRepository.CheckApartmentResidentAsync(request.ApartmentId, request.ResidentId, cancellationToken);
                if (apartmentMap == null)
                {
                    throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
                }

                //var spec = new ResidentFilterWithPagingSpec(request);
                FilterPagination filterPagination = new FilterPagination();
                filterPagination.page = request.page;
                filterPagination.page_size = request.page_size;
                filterPagination.query = request.query;
                filterPagination.order_by = request.order_by;
                filterPagination.search = request.search;
                var entities = await _entityRepository.GetMemberApartmentAdminAsync(filterPagination, request.ApartmentId, apartmentMap.Type, cancellationToken);

                //List<Apartment> res = new List<Apartment>();
                //foreach (var item in entities.Results)
                //{
                //    Apartment residentDTO = new ResResidentDT();
                //    residentDTO.ResidentId = item.ResidentId;
                //    residentDTO.FullName = item.FullName;
                //    residentDTO.Phone = item.Phone;
                    
                //    res.Add(residentDTO);
                //}
                //entities.Results = res;
                return entities;
            }
        }
    }
}
