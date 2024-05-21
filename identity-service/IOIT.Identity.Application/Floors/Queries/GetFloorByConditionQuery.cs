using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Floors.Queries
{
    public class GetFloorByConditionQuery : IRequest<Floor>
    {
        public string condition { get; set; }

        public class Handler : IRequestHandler<GetFloorByConditionQuery, Floor>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IFloorAsyncRepository _dataRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IFloorAsyncRepository dataRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _dataRepository = dataRepository;
            }

            public async Task<Floor> Handle(GetFloorByConditionQuery request, CancellationToken cancellationToken)
            {
                var entity = await _dataRepository.GetDataByCondition(request.condition);

                if (entity == null)
                {
                    throw new BadRequestException("Condition invalid.", Constants.StatusCodeResApi.Error400, ApiConstants.ErrorCode.ERROR_FLOOR_NOT_EXIST);
                }

                return entity;
            }
        }
    }
}
