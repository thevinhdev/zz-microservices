using AutoMapper;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Apartments.Queries
{
    public class GetCountApartmentWithTypeResidentQuery : IRequest<ResGetCountApartmentWithTypeResident>
    {
        public int? ProjectId { get; set; }
        public int ProjectTokenId { get; set; }
        public int UserType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public class Handler : IRequestHandler<GetCountApartmentWithTypeResidentQuery, ResGetCountApartmentWithTypeResident>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;
            private readonly IApartmentMapAsyncRepository _apartmentMapRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository,
                IApartmentMapAsyncRepository apartmentMapRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _apartmentMapRepository = apartmentMapRepository;
            }

            public async Task<ResGetCountApartmentWithTypeResident> Handle(GetCountApartmentWithTypeResidentQuery request, CancellationToken cancellationToken)
            {
                var entity = await _apartmentMapRepository.GetCountApartmentWithTypeResidentAsync(request.ProjectId, request.ProjectTokenId, request.UserType, request.FromDate, request.ToDate);

                return entity;
            }
        }
    }
}
