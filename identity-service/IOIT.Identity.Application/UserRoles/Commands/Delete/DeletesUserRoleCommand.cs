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
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.UserRoles.Commands.Delete
{
    public class DeletesUserRoleCommand : IRequest<List<User>>
    {
        public long[] data { get; set; }
        public long UserId { get; set; }

        public class Validation : AbstractValidator<DeletesUserRoleCommand>
        {
            public Validation()
            {
                RuleFor(x => x.data).NotEmpty().WithMessage("data not empty");
            }
        }
        public class Handler : IRequestHandler<DeletesUserRoleCommand, List<User>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _entityRepository;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository entityRepository,
                IFunctionRoleAsyncRepository funcRoleRepo,
                IUserRoleAsyncRepository userRoleRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _funcRoleRepo = funcRoleRepo;
                _userRoleRepo = userRoleRepo;
            }

            public async Task<List<User>> Handle(DeletesUserRoleCommand request, CancellationToken cancellationToken)
            {
                List<User> listData = new List<User>();
                for (int i = 0; i < request.data.Count(); i++)
                {
                    User user = await _entityRepository.GetByKeyAsync(request.data[i]);

                    if (user == null)
                    {
                        continue;
                    }

                    if (user.Id == 1)
                    {
                        continue;
                    }

                    //user.UpdatedAt = DateTime.Now;
                    //user.Status = (int)Const.Status.DELETED;
                    //db.Entry(user).State = EntityState.Modified;

                    ////delete user role
                    //var userRoles = db.UserRole.Where(e => e.UserId == user.UserId && e.Status != (int)Const.Status.DELETED).ToList();
                    //foreach (var item in userRoles)
                    //{
                    //    item.Status = (int)Const.Status.DELETED;
                    //    db.Entry(item).State = EntityState.Modified;
                    //}

                    ////delete function role
                    //var functionRoles = db.FunctionRole.Where(e => e.TargetId == user.UserId
                    //&& e.Type == (int)Const.TypeFunction.FUNCTION_USER
                    //&& e.Status != (int)Const.Status.DELETED).ToList();
                    //foreach (var item in functionRoles)
                    //{
                    //    item.Status = (int)Const.Status.DELETED;
                    //    item.UpdatedAt = DateTime.Now;
                    //    db.Entry(item).State = EntityState.Modified;
                    //}
                    user.UpdatedAt = DateTime.Now;
                    user.UpdatedById = request.UserId;
                    user.Status = EntityStatus.DELETED;

                    _entityRepository.Update(user);
                    await _unitOfWork.CommitChangesAsync();

                    //delete user role
                    //var userRoles = db.UserRole.Where(e => e.UserId == id && e.Status != (int)Const.Status.DELETED).ToList();
                    var userRoles = await _userRoleRepo.GetListUserRoleAsync(user.Id, cancellationToken);
                    foreach (var item in userRoles)
                    {
                        item.UpdatedAt = DateTime.Now;
                        item.UpdatedById = request.UserId;
                        item.Status = AppEnum.EntityStatus.DELETED;
                        //db.Entry(item).State = EntityState.Modified;
                    }
                    _userRoleRepo.UpdateRange(userRoles);

                    //delete function role
                    //var functionRoles = db.FunctionRole.Where(e => e.TargetId == id
                    //&& e.Type == (int)Const.TypeFunction.FUNCTION_USER
                    //&& e.Status != (int)Const.Status.DELETED).ToList();
                    var functionRoles = await _funcRoleRepo.GetListFunctionRoleAsync(user.Id, (int)AppEnum.TypeFunction.FUNCTION_USER, cancellationToken);
                    foreach (var item in functionRoles)
                    {

                        item.UpdatedAt = DateTime.Now;
                        item.UpdatedById = request.UserId;
                        item.Status = AppEnum.EntityStatus.DELETED;
                        //db.Entry(item).State = EntityState.Modified;
                    }
                    _funcRoleRepo.UpdateRange(functionRoles);

                    //lưu log
                    //Models.EF.Action action = new Models.EF.Action();
                    //action.ActionId = Guid.NewGuid();
                    //action.ActionName = "Xóa tài khoản " + user.UserName;
                    //action.ActionType = "DELETE";
                    //action.TargetId = user.UserId.ToString();
                    //action.TargetType = "User";
                    ////action.Logs = JsonConvert.SerializeObject(user);
                    //action.Time = 0;
                    //action.Type = (int)Const.TypeAction.ACTION;
                    //action.CreatedAt = DateTime.Now;
                    //action.UserPushId = userId;
                    //action.UserId = userId;
                    //action.Status = (int)Const.Status.NORMAL;
                    //db.Action.Add(action);
                    listData.Add(user);
                }

                await _unitOfWork.CommitChangesAsync();

                return listData;
            }
        }
    }
}
