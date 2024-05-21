using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Functions.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Functions.Queries
{
    class GetListFunctionRole : IRequest<Function>
    {
        public class Handler : IRequestHandler<GetListFunction, Function>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Function> _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Function> entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<Function> Handle(GetListFunction request, CancellationToken cancellationToken)
            {
                var entity = await _entityRepository.GetByKeyAsync(request.Id);

                if (entity == null)
                {
                    throw new BadRequestException("The Function does not exist.", Constants.StatusCodeResApi.Error404);
                }

                return entity;
            }

            private List<ResSmallFunction> listFunction(List<ResSmallFunction> dt, int functionId, int level, int roleMax, )
            {
                var index = level + 1;
                try
                {
                    using (var db = new IOITResidentGateContext())
                    {
                        IEnumerable<Function> data;
                        if (roleMax == 1)
                        {
                            data = db.Function.Where(e => e.FunctionParentId == functionId && e.Status != (int)AppEnum.EntityStatus.DELETED).ToList();
                        }
                        else
                        {
                            data = (from fr in db.FunctionRole
                                    join f in db.Function on fr.FunctionId equals f.FunctionId
                                    where fr.TargetId == roleMax && fr.Type == (int)AppEnum.TypeFunction.FUNCTION_ROLE
                                    && fr.Status != (int)AppEnum.EntityStatus.DELETED && fr.Status != (int)AppEnum.EntityStatus.DELETED
                                    && f.FunctionParentId == functionId
                                    select f).ToList();
                        }

                        if (data != null)
                        {
                            if (data.Count() > 0)
                            {
                                foreach (var item in data)
                                {
                                    ResSmallFunction function = new ResSmallFunction();
                                    function.FunctionId = item.Id;
                                    function.Code = item.Code;
                                    function.Name = item.Name;
                                    function.Level = level;
                                    //function.children = listFunction(item.FunctionId);
                                    dt.Add(function);
                                    try
                                    {
                                        listFunction(dt, item.Id, index, roleMax);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch { }

                return dt;
            }

        }
    }
}
