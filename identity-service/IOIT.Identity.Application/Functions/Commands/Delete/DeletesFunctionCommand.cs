using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
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
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Functions.Commands.Delete
{
    public class DeletesFunctionCommand : IRequest<List<Function>>
    {
        public int[] data { get; set; }

        public class Validation : AbstractValidator<DeletesFunctionCommand>
        {
            public Validation()
            {
                RuleFor(x => x.data).NotEmpty().WithMessage("data not empty");
            }
        }
        public class Handler : IRequestHandler<DeletesFunctionCommand, List<Function>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IFunctionAsyncRepository _entityRepository;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IFunctionAsyncRepository entityRepository,
                IFunctionRoleAsyncRepository funcRoleRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _funcRoleRepo = funcRoleRepo;
            }

            public async Task<List<Function>> Handle(DeletesFunctionCommand request, CancellationToken cancellationToken)
            {
                List<Function> listData = new List<Function>();
                for (int i = 0; i < request.data.Count(); i++)
                {
                    //Position position = await db.Position.Where(e => e.PositionId == data[i] && e.Status != (int)Const.Status.DELETED).FirstOrDefaultAsync();
                    Function function = await _entityRepository.GetByKeyAsync(request.data[i]);
                    if (function == null)
                    {
                        continue;
                    }

                    //hasDelete = true;
                    function.UpdatedAt = DateTime.Now;
                    function.Status = AppEnum.EntityStatus.DELETED;
                    //db.Position.Update(position);
                    var entity1 = _mapper.Map<Function>(function);
                    _entityRepository.Update(entity1);

                    //Xóa function role
                    //var fr = db.FunctionRole.Where(e => e.FunctionId == function.FunctionId).ToList();
                    var fr = await _funcRoleRepo.GetListFunctionRoleByIdAsync(function.Id, cancellationToken);
                    fr.ForEach(f => f.Status = AppEnum.EntityStatus.DELETED);
                    _funcRoleRepo.UpdateRange(fr);

                    //var listChild = db.Function.Where(f => f.FunctionParentId == function.FunctionId && f.Status != (int)Const.Status.DELETED).ToList();
                    var listChild = await _entityRepository.GetListFunctionByParentIdAsync(function.Id ,cancellationToken);
                    listChild.ForEach(c => c.FunctionParentId = 0);
                    _entityRepository.UpdateRange(listChild);

                    //create action
                    //IOITResident.Models.EF.Action action = new IOITResident.Models.EF.Action();
                    //action.ActionId = Guid.NewGuid();
                    //action.ActionName = "Xóa chức vụ " + position.Name;
                    //action.ActionType = "DELETE";
                    //action.TargetId = position.PositionId.ToString();
                    //action.TargetType = "Position";
                    //action.Logs = JsonConvert.SerializeObject(position);
                    //action.Time = 0;
                    //action.Type = (int)Const.TypeAction.ACTION;
                    //action.CreatedAt = DateTime.Now;
                    //action.UserPushId = userId;
                    //action.UserId = userId;
                    //action.Status = (int)Const.Status.NORMAL;
                    //db.Action.Add(action);
                    listData.Add(function);
                }

                await _unitOfWork.CommitChangesAsync();

                return listData;
            }
        }
    }
}
