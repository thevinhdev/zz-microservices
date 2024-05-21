using AutoMapper;
using IOIT.Identity.Application.Functions.ViewModels;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Functions.Queries
{
    public class GetFunctionByPagingQuery : FilterPagination, IRequest<ResFunctionLists>
    {
        public class Handler : IRequestHandler<GetFunctionByPagingQuery, ResFunctionLists>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IFunctionAsyncRepository _entityRepository;
            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IFunctionAsyncRepository entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<ResFunctionLists> Handle(GetFunctionByPagingQuery request, CancellationToken cancellationToken)
            {
                var spec = new FunctionFilterWithPagingSpec(request);

                var entities = await _entityRepository.PaggingAsync(spec);
                
                var listFunction = await _entityRepository.GetListAllFunctionAsync(cancellationToken);
                List<ResFunctionList> listFunctions = new List<ResFunctionList>();
                foreach(var item in entities.Results)
                {
                    ResFunctionList entity = new ResFunctionList();
                    entity.FunctionId = item.Id;
                    entity.Code = item.Code;
                    entity.Name = item.Name;
                    entity.Note = item.Note;
                    entity.Status = (int)item.Status;
                    entity.Url = item.Url;
                    entity.Icon = item.Icon;
                    entity.FunctionParentId = item.FunctionParentId;
                    entity.Location = item.Location;
                    entity.IsParamRoute = item.IsParamRoute;
                    entity.functionParent = listFunction.Where(e => e.Id == item.FunctionParentId).Select(e => new ResSmallFunction
                    {
                        FunctionId = e.Id,
                        Name = e.Name,
                        Code = e.Code,
                    }).FirstOrDefault();
                    listFunctions.Add(entity);
                }
                ResFunctionLists resFunctionLists = new ResFunctionLists();
                resFunctionLists.Results = listFunctions;
                resFunctionLists.RowCount = entities.RowCount;
                return resFunctionLists;
            }
        }
    }
}
