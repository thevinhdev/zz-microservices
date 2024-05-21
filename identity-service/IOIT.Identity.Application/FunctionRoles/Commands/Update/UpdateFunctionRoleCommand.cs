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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.FunctionRoles.Commands.Update
{
    public class UpdateFunctionRoleCommand : IRequest<Role>
    {
        public int Id { get; set; }
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

        public class Validation : AbstractValidator<UpdateFunctionRoleCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Code).NotEmpty().WithMessage("Code not empty");
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateFunctionRoleCommand, Role>
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

            public async Task<Role> Handle(UpdateFunctionRoleCommand request, CancellationToken cancellationToken)
            {
                //var data = await _entityRepository.GetByKeyAsync(request.Id);

                //if (data == null)
                //{
                //    throw new BadRequestException("The FunctionRole does not exist.", Constants.StatusCodeResApi.Error404);
                //}

                //var entity = _mapper.Map<FunctionRole>(request);
                //entity.UpdatedAt = DateTime.Now;
                //entity.UpdatedById = request.UserId;

                //_entityRepository.Update(entity);
                //await _unitOfWork.CommitChangesAsync();
                //using (var db = new IOITResidentGateContext())
                //{
                if (request.Code == null)
                {
                    throw new CommonException("Mã không có dữ liệu!", 211, ApiConstants.ErrorCode.ERROR_FUNCTION_CODE_EMPTY);
                }
                if (request.Code.Trim() == "")
                {
                    throw new CommonException("Mã không có dữ liệu!", 211, ApiConstants.ErrorCode.ERROR_FUNCTION_CODE_EMPTY);
                }

                var current = await _entityRepository.GetByKeyAsync(request.Id);
                if (current == null)
                {
                    throw new BadRequestException("Nhóm quyền không tồn tại trong hệ thống.", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_ROLE_NOT_EXIST);
                }

                //Role exist = db.Role.Where(f => f.Code == data.Code && f.Status != (int)Const.Status.DELETED && f.RoleId != id).FirstOrDefault();
                var exist = await _entityRepository.FindByCodeAsync(request.Code, request.Id, cancellationToken);
                if (exist != null)
                {
                    //def.meta = new Meta(212, "Mã quyền đã tồn tại!");
                    //return Ok(def);
                    throw new CommonException("Mã quyền đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_FUNCTION_EXISTED);
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                current.Code = request.Code;
                current.Name = request.Name;
                current.Note = request.Note;
                current.UpdatedAt = DateTime.Now;
                current.UpdatedById = request.UserId;

                try
                {

                    //update list function
                    foreach (var item in request.listFunction)
                    {
                        //var functionNew = db.FunctionRole.Where(e => e.TargetId == data.RoleId
                        //&& e.FunctionId == item.FunctionId
                        //&& e.Type == (int)Const.TypeFunction.FUNCTION_ROLE
                        //&& e.Status != (int)Const.Status.DELETED).ToList();
                        var functionNew = await _funcRoleRepo.GetListFunctionRoleUpdateAsync(request.RoleId, item.FunctionId, (int)AppEnum.TypeFunction.FUNCTION_ROLE, cancellationToken);
                        //add new
                        if (functionNew.Count <= 0)
                        {
                            FunctionRole functionRole = new FunctionRole();
                            functionRole.TargetId = request.Id;
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
                        else
                        {
                            //update
                            var functionRoleExit = functionNew.FirstOrDefault();
                            functionRoleExit.ActiveKey = item.ActiveKey;
                            functionRoleExit.UpdatedAt = DateTime.Now;
                            functionRoleExit.UpdatedById = request.UserId;
                            //db.Entry(functionRoleExit).State = EntityState.Modified;
                            _funcRoleRepo.Update(functionRoleExit);
                        }
                    }

                    //db.Entry(current).State = EntityState.Modified;
                    //await db.SaveChangesAsync();
                    _entityRepository.Update(current);
                    await _unitOfWork.CommitChangesAsync();

                    if (current.Id > 0)
                    {
                        _unitOfWork.CommitTransaction();
                        //transaction.Commit();
                        ////create action
                        //Models.EF.Action action = new Models.EF.Action();
                        //action.ActionId = Guid.NewGuid();
                        //action.ActionName = "Sửa quyền " + data.Name;
                        //action.ActionType = "UPDATE";
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

                        //def.meta = new Meta(200, Const.UPDATE_SUCCESS_MESSAGE);
                        //def.data = data;
                        //return Ok(def);
                        return current;
                    }
                    else
                    {
                        //transaction.Rollback();
                        //def.meta = new Meta(500, Const.ERROR_500_MESSAGE);
                        //return Ok(def);
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_FUNCTION_UPDATE_FAILED);
                    }
                }
                catch (DbUpdateConcurrencyException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_FUNCTION_UPDATE_FAILED);
                }
            }
        }
    }
}
