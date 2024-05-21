using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.FunctionRoles.Commands.Delete
{
    public class DeleteFunctionRoleCommand : IRequest<Role>
    {
        public int Id { get; set; }
        public long? UserId { get; set; }

        public class Validation : AbstractValidator<DeleteFunctionRoleCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
            }


        }
        public class Handler : IRequestHandler<DeleteFunctionRoleCommand, Role>
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

            public async Task<Role> Handle(DeleteFunctionRoleCommand request, CancellationToken cancellationToken)
            {
                //var data = await _entityRepository.GetByKeyAsync(request.Id);

                //if (data == null)
                //{
                //    throw new NotFoundException("The FunctionRole does not exist.", Constants.StatusCodeResApi.Error404);
                //}

                //if (data.Status == EntityStatus.DELETED)
                //{
                //    throw new NotFoundException("The FunctionRole does not exist.", Constants.StatusCodeResApi.Error404);
                //}

                //data.UpdatedAt = DateTime.Now;
                //data.UpdatedById = request.UserId;
                //data.Status = EntityStatus.DELETED;

                //_entityRepository.Update(data);
                //await _unitOfWork.CommitChangesAsync();
                //using (var db = new IOITResidentGateContext())
                //{
                //Role data = await db.Role.FindAsync(id);
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new NotFoundException("The FunctionRole does not exist.", Constants.StatusCodeResApi.Error404);
                }

                if (data.Status == EntityStatus.DELETED)
                {
                    throw new NotFoundException("The FunctionRole does not exist.", Constants.StatusCodeResApi.Error404);
                }
                data.UpdatedAt = DateTime.Now;
                data.UpdatedById = request.UserId;
                data.Status = AppEnum.EntityStatus.DELETED;

                //using (var transaction = db.Database.BeginTransaction())
                //{
                //db.Entry(data).State = EntityState.Modified;
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                _entityRepository.Update(data);
                //var fr = db.FunctionRole.Where(e => e.TargetId == data.RoleId && e.Type == (int)Const.TypeFunction.FUNCTION_ROLE
                //            && e.Status != (int)Const.Status.DELETED).ToList();
                //db.FunctionRole.RemoveRange(fr);
                List<FunctionRole> fr = await _funcRoleRepo.GetListFunctionRoleAsync(data.Id, (int)AppEnum.TypeFunction.FUNCTION_ROLE, cancellationToken);
                _funcRoleRepo.DeleteRange(fr);
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
                        //action.ActionName = "Xóa quyền " + data.Name;
                        //action.ActionType = "DELETE";
                        //action.TargetId = data.RoleId.ToString();
                        //action.TargetType = "Role";
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
                        //def.data = data.RoleId;
                        //return Ok(def);
                        return data;
                    }
                    else
                    {
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_FUNCTION_UPDATE_FAILED);
                    }
                }
                catch (DbUpdateException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_FUNCTION_UPDATE_FAILED);
                }
                //}
                //}
            }
        }
    }
}
