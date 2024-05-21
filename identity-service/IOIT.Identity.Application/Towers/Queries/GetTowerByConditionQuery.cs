using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Towers.Queries
{
    public class GetTowerByConditionQuery : IRequest<Tower>
    {
        public string condition { get; set; }

        public class Handler : IRequestHandler<GetTowerByConditionQuery, Tower>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITowerAsyncRepository _dataRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                ITowerAsyncRepository dataRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _dataRepository = dataRepository;
            }

            public async Task<Tower> Handle(GetTowerByConditionQuery request, CancellationToken cancellationToken)
            {
                var entity = await _dataRepository.GetDataByCondition(request.condition);

                if (entity == null)
                {
                    throw new BadRequestException("Không có dữ liệu tòa nhà trong hệ thống", Constants.StatusCodeResApi.Error400, ApiConstants.ErrorCode.ERROR_RESIDENT_NOT_EXIST);
                }

                return entity;
            }
        }
    }
}
