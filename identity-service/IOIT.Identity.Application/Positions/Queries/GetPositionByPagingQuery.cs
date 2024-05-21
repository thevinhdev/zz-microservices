using AutoMapper;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Positions.Queries
{
    public class GetPositionByPagingQuery : FilterPagination, IRequest<IPagedResult<Position>>
    {
        public class Handler : IRequestHandler<GetPositionByPagingQuery, IPagedResult<Position>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Position> _entityRepository;
            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Position> entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<IPagedResult<Position>> Handle(GetPositionByPagingQuery request, CancellationToken cancellationToken)
            {
                var spec = new PositionFilterWithPagingSpec(request);

                var entities = await _entityRepository.PaggingAsync(spec);

                return entities;
            }
        }
    }
}
