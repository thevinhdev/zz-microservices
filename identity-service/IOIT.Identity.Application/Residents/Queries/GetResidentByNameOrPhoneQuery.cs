using AutoMapper;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Residents.Queries
{
    public class GetResidentByNameOrPhoneQuery : IRequest<ResGetResidentByNameAndPhoneInApartment>
    {
        public int ApartmentId { get; set; }
        public string ResidentName { get; set; }
        public string ResidentPhone { get; set; }

        public class Handler : IRequestHandler<GetResidentByNameOrPhoneQuery, ResGetResidentByNameAndPhoneInApartment>
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

            public async Task<ResGetResidentByNameAndPhoneInApartment> Handle(GetResidentByNameOrPhoneQuery request, CancellationToken cancellationToken)
            {
                var entity = await _entityRepository.GetResidentByNameOrPhoneInApartment(request.ApartmentId, request.ResidentName, request.ResidentPhone, cancellationToken);
                
                return entity;
            }
        }
    }
}
