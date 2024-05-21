using AutoMapper;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Functions.Queries
{
    public class GetListFunction : IRequest<List<ResSmallFunction>>
    {
        public int roleMax { get; set; }

        public class Handler : IRequestHandler<GetListFunction, List<ResSmallFunction>>
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

            public async Task<List<ResSmallFunction>> Handle(GetListFunction request, CancellationToken cancellationToken)
            {
                
                List<ResSmallFunction> functions = new List<ResSmallFunction>();
                await _entityRepository.GetListFunctionAsync(functions, 0, 0, request.roleMax,cancellationToken);

                return functions;
            }

            //private List<ResSmallFunction> listFunction(List<ResSmallFunction> dt, int functionId, int level, int roleMax, List<Function> listAllFunction)
            //{
            //    var index = level + 1;
            //    try
            //    {
            //        //using (var db = new IOITResidentGateContext())
            //        //{
            //            IEnumerable<Function> data;
            //            if (roleMax == 1)
            //            {
            //                data = listAllFunction.Where(e => e.FunctionParentId == functionId && e.Status != AppEnum.EntityStatus.DELETED).ToList();
            //            }
            //            else
            //            {
            //                data = (from fr in db.FunctionRole
            //                        join f in db.Function on fr.FunctionId equals f.FunctionId
            //                        where fr.TargetId == roleMax && fr.Type == (int)AppEnum.TypeFunction.FUNCTION_ROLE
            //                        && fr.Status != (int)AppEnum.EntityStatus.DELETED && fr.Status != (int)AppEnum.EntityStatus.DELETED
            //                        && f.FunctionParentId == functionId
            //                        select f).ToList();
            //            }

            //            if (data != null)
            //            {
            //                if (data.Count() > 0)
            //                {
            //                    foreach (var item in data)
            //                    {
            //                        ResSmallFunction function = new ResSmallFunction();
            //                        function.FunctionId = item.Id;
            //                        function.Code = item.Code;
            //                        function.Name = item.Name;
            //                        function.Level = level;
            //                        //function.children = listFunction(item.FunctionId);
            //                        dt.Add(function);
            //                        try
            //                        {
            //                            listFunction(dt, item.Id, index, roleMax);
            //                        }
            //                        catch { }
            //                    }
            //                }
            //            }
            //        //}
            //    }
            //    catch { }

            //    return dt;
            //}

            //public List<ResSmallFunction> listFunction(List<ResSmallFunction> dt, int functionId, int level, int roleMax, )
            //{
            //    var index = level + 1;
            //    try
            //    {
            //        using (var db = new IOITResidentGateContext())
            //        {
            //            IEnumerable<Function> data;
            //            if (roleMax == 1)
            //            {
            //                data = db.Function.Where(e => e.FunctionParentId == functionId && e.Status != (int)AppEnum.EntityStatus.DELETED).ToList();
            //            }
            //            else
            //            {
            //                data = (from fr in db.FunctionRole
            //                        join f in db.Function on fr.FunctionId equals f.FunctionId
            //                        where fr.TargetId == roleMax && fr.Type == (int)AppEnum.TypeFunction.FUNCTION_ROLE
            //                        && fr.Status != (int)AppEnum.EntityStatus.DELETED && fr.Status != (int)AppEnum.EntityStatus.DELETED
            //                        && f.FunctionParentId == functionId
            //                        select f).ToList();
            //            }

            //            if (data != null)
            //            {
            //                if (data.Count() > 0)
            //                {
            //                    foreach (var item in data)
            //                    {
            //                        ResSmallFunction function = new ResSmallFunction();
            //                        function.FunctionId = item.Id;
            //                        function.Code = item.Code;
            //                        function.Name = item.Name;
            //                        function.Level = level;
            //                        //function.children = listFunction(item.FunctionId);
            //                        dt.Add(function);
            //                        try
            //                        {
            //                            listFunction(dt, item.Id, index, roleMax);
            //                        }
            //                        catch { }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch { }

            //    return dt;
            //}

        }
    }
}
