using AutoMapper;
using FluentValidation;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.FunctionRoles.Commands.Delete
{
    public class DeletesFunctionRoleCommand : IRequest<List<Role>>
    {
        public int[] data { get; set; }
        public long UserId { get; set; }

        public class Validation : AbstractValidator<DeletesFunctionRoleCommand>
        {
            public Validation()
            {
                RuleFor(x => x.data).NotEmpty().WithMessage("data not empty");
            }
        }
        public class Handler : IRequestHandler<DeletesFunctionRoleCommand, List<Role>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRoleAsyncRepository _entityRepository;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IRoleAsyncRepository entityRepository,
                IFunctionRoleAsyncRepository funcRoleRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _funcRoleRepo = funcRoleRepo;
            }

            public async Task<List<Role>> Handle(DeletesFunctionRoleCommand request, CancellationToken cancellationToken)
            {
                List<Role> listData = new List<Role>();
                for (int i = 0; i < request.data.Count(); i++)
                {
                    var role = await _entityRepository.GetByKeyAsync(request.data[i]);
                    if (role == null)
                    {
                        continue;
                    }
                    role.UpdatedAt = DateTime.Now;
                    role.UpdatedById = request.UserId;
                    role.Status = AppEnum.EntityStatus.DELETED;
                    //db.Entry(role).State = EntityState.Modified;
                    _entityRepository.Update(role);


                    //var fr = db.FunctionRole.Where(e => e.TargetId == role.RoleId && e.Type == (int)Const.TypeFunction.FUNCTION_ROLE
                    //    && e.Status != (int)Const.Status.DELETED).ToList();
                    //fr.ForEach(f => f.Status = (int)Const.Status.DELETED);
                    List<FunctionRole> fr = await _funcRoleRepo.GetListFunctionRoleAsync(request.data[i], (int)AppEnum.TypeFunction.FUNCTION_ROLE, cancellationToken);
                    _funcRoleRepo.DeleteRange(fr);

                    ////lưu log
                    //Models.EF.Action action = new Models.EF.Action();
                    //action.ActionId = Guid.NewGuid();
                    //action.ActionName = "Xóa quyền " + role.Name;
                    //action.ActionType = "DELETE";
                    //action.TargetId = role.RoleId.ToString();
                    //action.TargetType = "Role";
                    ////action.Logs = JsonConvert.SerializeObject(role);
                    //action.Time = 0;
                    //action.Type = (int)Const.TypeAction.ACTION;
                    //action.CreatedAt = DateTime.Now;
                    //action.UserPushId = userId;
                    //action.UserId = userId;
                    //action.Status = (int)Const.Status.NORMAL;
                    //db.Action.Add(action);
                    listData.Add(role);
                }

                await _unitOfWork.CommitChangesAsync();

                return listData;
            }
        }
    }
}
