using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.UserRoles.Commands.Delete
{
    public class DeleteUserRoleCommand : IRequest<User>
    {
        public long Id { get; set; }
        public long? UserId { get; set; }

        public class Validation : AbstractValidator<DeleteUserRoleCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
            }

            
        }
        public class Handler : IRequestHandler<DeleteUserRoleCommand, User>
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

            public async Task<User> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
            {
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new NotFoundException("The User does not exist.", Constants.StatusCodeResApi.Error404);
                }

                if (data.Status == EntityStatus.DELETED)
                {
                    throw new NotFoundException("The User does not exist.", Constants.StatusCodeResApi.Error404);
                }

                if (request.Id == 1)
                {
                    //def.meta = new Meta(210, "Không thể xóa tài khoản này!");
                    //return Ok(def);
                    throw new CommonException("Không thể xóa tài khoản này!", 210, ApiConstants.ErrorCode.ERROR_USER_DELETE_FAILED);
                }

                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);

                data.UpdatedAt = DateTime.Now;
                data.UpdatedById = request.UserId;
                data.Status = EntityStatus.DELETED;

                _entityRepository.Update(data);
                await _unitOfWork.CommitChangesAsync();

                //delete user role
                //var userRoles = db.UserRole.Where(e => e.UserId == id && e.Status != (int)Const.Status.DELETED).ToList();
                var userRoles = await _userRoleRepo.GetListUserRoleAsync(request.Id, cancellationToken);
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
                var functionRoles = await _funcRoleRepo.GetListFunctionRoleAsync(request.Id, (int)AppEnum.TypeFunction.FUNCTION_USER, cancellationToken);
                foreach (var item in functionRoles)
                {
                    
                    item.UpdatedAt = DateTime.Now;
                    item.UpdatedById = request.UserId;
                    item.Status = AppEnum.EntityStatus.DELETED;
                    //db.Entry(item).State = EntityState.Modified;
                }
                _funcRoleRepo.UpdateRange(functionRoles);

                try
                {
                    await _unitOfWork.CommitChangesAsync();
                    if (data.Id > 0)
                    {
                        _unitOfWork.CommitTransaction();
                        //transaction.Commit();
                        ////create action
                        //Models.EF.Action action = new Models.EF.Action();
                        //action.ActionId = Guid.NewGuid();
                        //action.ActionName = "Xóa tài khoản " + data.UserName;
                        //action.ActionType = "DELETE";
                        //action.TargetId = data.UserId.ToString();
                        //action.TargetType = "User";
                        //action.Logs = "";
                        //action.Time = 0;
                        //action.Type = (int)Const.TypeAction.ACTION;
                        //action.CreatedAt = DateTime.Now;
                        //action.UserPushId = userId;
                        //action.UserId = userId;
                        //action.Status = (int)Const.Status.NORMAL;
                        //db.Action.Add(action);

                        //await db.SaveChangesAsync();

                        //def.meta = new Meta(200, Const.DELETE_SUCCESS_MESSAGE);
                        //def.data = id;
                        //return Ok(def);
                        return data;
                    }
                    else
                    {
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_USERROLE_DELETE_FAILED);
                    }
                }
                catch (DbUpdateConcurrencyException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_USERROLE_DELETE_FAILED);
                }

                
            }
        }
    }
}
