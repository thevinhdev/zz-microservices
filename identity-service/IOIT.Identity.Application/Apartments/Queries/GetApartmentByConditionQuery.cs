using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Apartments.Queries
{
    public class GetApartmentByConditionQuery : IRequest<Apartment>
    {
        public string condition { get; set; }

        public class Handler : IRequestHandler<GetApartmentByConditionQuery, Apartment>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApartmentAsyncRepository _dataRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IApartmentAsyncRepository dataRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _dataRepository = dataRepository;
            }

            public async Task<Apartment> Handle(GetApartmentByConditionQuery request, CancellationToken cancellationToken)
            {
                var entity = await _dataRepository.GetDataByCondition(request.condition);

                if (entity == null)
                {
                    throw new BadRequestException("Condition invalid.", Constants.StatusCodeResApi.Error400);
                }

                return entity;
            }
        }
    }
}
