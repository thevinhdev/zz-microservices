using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Application.Utilities.ViewModels;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;

namespace IOIT.Identity.Application.Utilities.Queries
{
    public class GetUtilitiesByPageQuery : FilterPagination, IRequest<IPagedResult<ResGetUtilitiesById>>
    {
        public class Handler : IRequestHandler<GetUtilitiesByPageQuery, IPagedResult<ResGetUtilitiesById>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUtilitiesRepository _utilitiesRepository;
            private readonly IProjectUtilitiesRepository _projectUtilitiesRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUtilitiesRepository utilitiesRepository,
                IProjectUtilitiesRepository projectUtilitiesRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _utilitiesRepository = utilitiesRepository;
                _projectUtilitiesRepository = projectUtilitiesRepository;
            }

            public async Task<IPagedResult<ResGetUtilitiesById>> Handle(GetUtilitiesByPageQuery request, CancellationToken cancellationToken)
            {
                var spec = new UtilitiesFilterWithPagingSpec(request);

                var entities = await _utilitiesRepository.PaggingAsync(spec);

                var resData = _mapper.Map<IPagedResult<ResGetUtilitiesById>>(entities);

                foreach (var item in resData.Results)
                {
                    var listProjectId = await _projectUtilitiesRepository.getProjectIdByUtilitiesId(item.Id);

                    item.ListProjectId = listProjectId;
                }

                return resData;
            }
        }
    }
}
