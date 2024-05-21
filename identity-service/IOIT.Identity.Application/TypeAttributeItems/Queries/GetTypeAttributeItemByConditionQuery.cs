using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.TypeAttributeItems.Queries
{
    public class GetTypeAttributeItemByConditionQuery : IRequest<TypeAttributeItem>
    {
        public string condition { get; set; }

        public class Handler : IRequestHandler<GetTypeAttributeItemByConditionQuery, TypeAttributeItem>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITypeAttributeItemAsyncRepository _dataRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                ITypeAttributeItemAsyncRepository dataRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _dataRepository = dataRepository;
            }

            public async Task<TypeAttributeItem> Handle(GetTypeAttributeItemByConditionQuery request, CancellationToken cancellationToken)
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
