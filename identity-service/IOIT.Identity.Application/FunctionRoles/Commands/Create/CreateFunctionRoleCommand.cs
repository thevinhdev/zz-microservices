using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.UserRoles.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.FunctionRoles.Commands.Create
{
    public class CreateFunctionRoleCommand : IRequest<Role>
    {
        public int RoleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public byte? LevelRole { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public int? UserEditId { get; set; }
        public int? Status { get; set; }
        public List<FunctionRoleDT> listFunction { get; set; }

        public class Validation : AbstractValidator<CreateFunctionRoleCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<CreateFunctionRoleCommand, Role>
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

            public async Task<Role> Handle(CreateFunctionRoleCommand request, CancellationToken cancellationToken)
            {
                //var entity = _mapper.Map<FunctionRole>(request);

                //await _entityRepository.AddAsync(entity);
                //await _unitOfWork.CommitChangesAsync();
                //Role exist = db.Role.Where(f => f.Code == data.Code && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                if (request.Code == null || request.Code.Trim() == "")
                {
                    throw new BadRequestException("Vui lòng nhập mã quyền.", ApiConstants.StatusCode.Valid211, ApiConstants.ErrorCode.ERROR_FUNCTION_CODE_EMPTY);
                }

                var exist = await _entityRepository.FindByCodeAsync(request.Code, 0, cancellationToken);
                if (exist != null)
                {
                    throw new CommonException("Mã quyền đã tồn tại!", ApiConstants.StatusCode.Error400, ApiConstants.ErrorCode.ERROR_FUNCTION_EXISTED);
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                Role role = new Role();
                role.Code = request.Code;
                role.Name = request.Name;
                role.Note = request.Note;
                role.CreatedAt = DateTime.Now;
                role.UpdatedAt = DateTime.Now;
                role.CreatedById = request.UserId;
                role.UpdatedById = request.UserId;
                role.Status = AppEnum.EntityStatus.NORMAL;
                //db.Role.Add(role);
                await _entityRepository.AddAsync(role);
                try
                {
                    await _unitOfWork.CommitChangesAsync();

                    request.RoleId = role.Id;

                    //add function
                    foreach (var item in request.listFunction)
                    {
                        FunctionRole functionRole = new FunctionRole();
                        functionRole.TargetId = role.Id;
                        functionRole.FunctionId = item.FunctionId;
                        functionRole.ActiveKey = item.ActiveKey;
                        functionRole.Type = (int)AppEnum.TypeFunction.FUNCTION_ROLE;
                        functionRole.CreatedAt = DateTime.Now;
                        functionRole.UpdatedAt = DateTime.Now;
                        functionRole.CreatedById = request.UserId;
                        functionRole.UpdatedById = request.UserId;
                        functionRole.Status = AppEnum.EntityStatus.NORMAL;
                        //db.FunctionRole.Add(functionRole);
                        await _funcRoleRepo.AddAsync(functionRole);
                    }
                    //await db.SaveChangesAsync();
                    await _unitOfWork.CommitChangesAsync();
                    if (role.Id > 0)
                    {
                        _unitOfWork.CommitTransaction();
                        //transaction.Commit();
                        ////create action
                        //Models.EF.Action action = new Models.EF.Action();
                        //action.ActionId = Guid.NewGuid();
                        //action.ActionName = "Thêm quyền " + data.Name;
                        //action.ActionType = "CREATE";
                        //action.TargetId = data.RoleId.ToString();
                        //action.TargetType = "Role";
                        //action.Logs = JsonConvert.SerializeObject(data);
                        //action.Time = 0;
                        //action.Type = (int)Const.TypeAction.ACTION;
                        //action.CreatedAt = DateTime.Now;
                        //action.UserPushId = userId;
                        //action.UserId = userId;
                        //action.Status = (int)Const.Status.NORMAL;
                        //db.Action.Add(action);

                        //await db.SaveChangesAsync();

                        //def.meta = new Meta(200, Const.CREATE_SUCCESS_MESSAGE);
                        //def.data = data;
                        //return Ok(def);
                        return role;
                    }
                    else
                    {
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_FUNCTION_CREATE_FAILED);
                    }
                }
                catch (DbUpdateException ex)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(ex.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_FUNCTION_CREATE_FAILED);
                }
            }

            //return entity;
        }
    }
}

