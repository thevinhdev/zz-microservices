using AutoMapper;
using IOIT.Identity.Application.FunctionRoles.ViewModels;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.FunctionRoles.Queries
{
    public class GetFunctionRoleByPagingQuery : FilterPagination, IRequest<ResFunctionRoleLists>
    {
        public class Handler : IRequestHandler<GetFunctionRoleByPagingQuery, ResFunctionRoleLists>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Role> _entityRepository;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;
            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Role> entityRepository,
                IFunctionRoleAsyncRepository funcRoleRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _funcRoleRepo = funcRoleRepo;
            }

            public async Task<ResFunctionRoleLists> Handle(GetFunctionRoleByPagingQuery request, CancellationToken cancellationToken)
            {
                var spec = new FunctionRoleFilterWithPagingSpec(request);

                var entities = await _entityRepository.PaggingAsync(spec);
                List<ResFunctionRoleDT> listFunctionRoles = new List<ResFunctionRoleDT>();
                foreach (var item in entities.Results)
                {
                    ResFunctionRoleDT entity = new ResFunctionRoleDT();

                    entity.RoleId = item.Id;
                    entity.Code = item.Code;
                    entity.Name = item.Name;
                    entity.Note = item.Note;
                    entity.Status = (int)item.Status;
                    var listFR = await _funcRoleRepo.GetListFunctionRoleAsync(item.Id, (int)AppEnum.TypeFunction.FUNCTION_ROLE, cancellationToken);
                    List<ListFunctionDT> listFunctions = new List<ListFunctionDT>();
                    foreach(var itemFR in listFR)
                    {
                        ListFunctionDT functionDT = new ListFunctionDT();
                        functionDT.FunctionId = itemFR.FunctionId;
                        functionDT.ActiveKey = itemFR.ActiveKey;
                        listFunctions.Add(functionDT);
                    }
                    entity.listFunction = listFunctions;
                    listFunctionRoles.Add(entity);
                }
                ResFunctionRoleLists resFunctionLists = new ResFunctionRoleLists();
                resFunctionLists.Results = listFunctionRoles;
                resFunctionLists.RowCount = entities.RowCount;
                return resFunctionLists;
            }
        }
    }
}
