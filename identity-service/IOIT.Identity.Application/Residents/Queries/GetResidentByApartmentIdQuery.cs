using AutoMapper;
using IOIT.Identity.Application.Residents.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Residents.Queries
{
    public class GetResidentByApartmentIdQuery : IRequest<IPagedResult<ResGetResidentByApartmentId>>
    {
        public int ApartmentId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public class Handler : IRequestHandler<GetResidentByApartmentIdQuery, IPagedResult<ResGetResidentByApartmentId>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<IPagedResult<ResGetResidentByApartmentId>> Handle(GetResidentByApartmentIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _entityRepository.GetByApartmentIdAndPageAsync(request.ApartmentId, null, request.PageSize, request.PageIndex, cancellationToken);

                return entity;
            }
        }
    }
}
