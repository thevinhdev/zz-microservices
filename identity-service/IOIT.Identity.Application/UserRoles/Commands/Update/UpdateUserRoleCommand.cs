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

namespace IOIT.Identity.Application.UserRoles.Commands.Update
{
    public class UpdateUserRoleCommand : IRequest<User>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public string Avata { get; set; }
        public DateTime? Birthday { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CardId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public string KeyRandom { get; set; }
        public byte? TypeThird { get; set; }
        public long? UserMapId { get; set; }
        public byte? Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string RegEmail { get; set; }
        public int? RoleMax { get; set; }
        public byte? RoleLevel { get; set; }
        public bool? IsRoleGroup { get; set; }
        public bool? IsPhoneConfirm { get; set; }
        public bool? IsEmailConfirm { get; set; }
        public string RegisterCode { get; set; }
        public int? CountLogin { get; set; }
        public int? LanguageId { get; set; }
        public int? UserCreateId { get; set; }
        public int? UserEditId { get; set; }
        public byte? Status { get; set; }
        public List<RoleDT> listRole { get; set; }
        public List<FunctionRoleDT> listFunction { get; set; }
        public List<ListProject> listProject { get; set; }
        public List<ListTower> listTower { get; set; }

        public class Validation : AbstractValidator<UpdateUserRoleCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.FullName).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateUserRoleCommand, User>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _entityRepository;
            private readonly IEmployeeAsyncRepository _empRepo;
            private readonly IResidentAsyncRepository _residentRepo;
            private readonly IRoleAsyncRepository _roleRepo;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                 IUserAsyncRepository entityRepository,
                IEmployeeAsyncRepository empRepo,
                IResidentAsyncRepository residentRepo,
                IRoleAsyncRepository roleRepo,
                IFunctionRoleAsyncRepository funcRoleRepo,
                IUserRoleAsyncRepository userRoleRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _empRepo = empRepo;
                _residentRepo = residentRepo;
                _roleRepo = roleRepo;
                _funcRoleRepo = funcRoleRepo;
                _userRoleRepo = userRoleRepo;
            }

            public async Task<User> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
            {
                //if (request.Code == null || request.Code == "")
                //{
                //    throw new CommonException("Mã không có dữ liệu!", 211, ApiConstants.ErrorCode.ERROR_CODE_EMPTY);
                //}
                var current = await _entityRepository.GetByKeyAsync(request.Id);

                if (current == null)
                {
                    throw new BadRequestException("Tài khoản không tồn tại trong hệ thống", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                //var entity = _mapper.Map<UserRole>(request);
                //entity.UpdatedAt = DateTime.Now;
                //entity.UpdatedById = request.UserId;

                //_entityRepository.Update(entity);
                //await _unitOfWork.CommitChangesAsync();

                //return entity;
                //User checkUserNameExist = db.User.Where(f => f.UserId != data.UserId && f.UserName == data.UserName && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                var checkUserNameExist = await _entityRepository.CheckUserNameExitAsync(request.UserName, request.Id, cancellationToken);
                if (checkUserNameExist != null)
                {
                    //def.meta = new Meta(211, "Tên tài khoản đã tồn tại!");
                    //return Ok(def);
                    throw new CommonException("Tên tài khoản đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_USERNAME_EXISTED);
                }

                //User checkEmailExist = db.User.Where(f => f.UserId != data.UserId && f.Email == data.Email && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                var checkEmailExist = await _entityRepository.FindByEmailAsync(request.UserName, request.Id, cancellationToken);
                if (checkEmailExist != null)
                {
                    //def.meta = new Meta(211, "Email đã tồn tại!");
                    //return Ok(def);
                    throw new CommonException("Email đã tồn tại!", 212 , ApiConstants.ErrorCode.ERROR_EMAIL_EXISTED);
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                //update user
                current.FullName = request.FullName;
                current.Code = request.Code;
                current.Phone = request.Phone;
                current.Email = request.Email;
                current.Address = request.Address;
                current.Avata = request.Avata;
                current.DepartmentId = request.DepartmentId;
                current.PositionId = request.PositionId;
                current.IsRoleGroup = request.IsRoleGroup != null ? request.IsRoleGroup : true;
                current.UpdatedAt = DateTime.Now;
                current.UpdatedById = request.UserId;
                //db.Entry(current).State = EntityState.Modified;
                _entityRepository.Update(current);
                try
                {
                    //role old
                    byte levelOld = (byte)current.RoleLevel;
                    // role
                    var checkRole = false;
                    byte level = 99;
                    int max = 9999;
                    //update list role
                    //add new
                    foreach (var item in request.listRole)
                    {
                        //var role = db.Role.Find(item.RoleId);
                        var role = await _roleRepo.GetByKeyAsync(item.RoleId);
                        if (role != null)
                        {
                            //var userRoleNew = db.UserRole.Where(e => e.UserId == data.UserId && e.RoleId == item.RoleId && e.Status != (int)Const.Status.DELETED).ToList();
                            var userRoleNew = await _userRoleRepo.GetListUserRoleNewAsync(request.Id, item.RoleId, cancellationToken);
                            if (userRoleNew.Count <= 0)
                            {
                                UserRole userRole = new UserRole();
                                userRole.RoleId = item.RoleId;
                                userRole.UserId = request.Id;
                                userRole.CreatedAt = DateTime.Now;
                                userRole.UpdatedAt = DateTime.Now;
                                userRole.CreatedById = request.UserId;
                                userRole.UpdatedById = request.UserId;
                                userRole.Status = AppEnum.EntityStatus.NORMAL;
                                //db.UserRole.Add(userRole);
                                await _userRoleRepo.AddAsync(userRole);
                            }
                            //check role
                            if (role.Code.Trim() == "ADMIN" || role.Code.Trim() == "MANAGER" || role.Code.Trim() == "USER" || role.Code.Trim() == "MANAGER_FULL")
                                checkRole = true;
                            //
                            if (role.LevelRole < level)
                            {
                                level = (byte)role.LevelRole;
                                max = role.Id;
                            }
                        }
                    }
                    //delete old
                    //var listUserRole = db.UserRole.Where(e => e.UserId == data.UserId && e.Status != (int)Const.Status.DELETED).ToList();
                    var listUserRole = await _userRoleRepo.GetListUserRoleAsync(request.Id, cancellationToken);
                    foreach (var item in listUserRole)
                    {
                        var listNew = request.listRole.Where(e => e.RoleId == item.RoleId).ToList();
                        if (listNew.Count <= 0)
                        {
                            //UserRole userRoleExit = await db.UserRole.FindAsync(item.UserRoleId);
                            UserRole userRoleExit = await _userRoleRepo.GetByKeyAsync(item.Id);
                            userRoleExit.UpdatedAt = DateTime.Now;
                            userRoleExit.UpdatedById = request.UserId;
                            userRoleExit.Status = AppEnum.EntityStatus.DELETED;
                            //db.Entry(userRoleExit).State = EntityState.Modified;
                            _userRoleRepo.Update(userRoleExit);
                        }
                        else
                        {
                            //Check xem có phải quyền giám sát ko
                            //var role = db.Role.Find(item.RoleId);
                            var role = await _roleRepo.GetByKeyAsync(item.RoleId);
                            if (role != null)
                            {
                                //check role
                                if (role.Code.Trim() == "ADMIN" || role.Code.Trim() == "MANAGER" || role.Code.Trim() == "USER" || role.Code.Trim() == "MANAGER_FULL")
                                    checkRole = true;
                            }
                        }
                    }

                    //update quyền cao nhất và cấp cao nhất của user
                    current.RoleLevel = level;
                    current.RoleMax = max;
                    //db.Entry(current).State = EntityState.Modified;
                    _entityRepository.Update(current);
                    //update list function
                    foreach (var item in request.listFunction)
                    {
                        //var functionNew = db.FunctionRole.Where(e => e.TargetId == data.UserId
                        //&& e.FunctionId == item.FunctionId
                        //&& e.Type == (int)Const.TypeFunction.FUNCTION_USER
                        //&& e.Status != (int)Const.Status.DELETED).ToList();
                        var functionNew = await _funcRoleRepo.GetListFunctionRoleUpdateAsync(request.Id, item.FunctionId, (int)AppEnum.TypeFunction.FUNCTION_USER, cancellationToken);
                        //add new
                        if (functionNew.Count <= 0)
                        {
                            FunctionRole functionRole = new FunctionRole();
                            functionRole.TargetId = request.Id;
                            functionRole.FunctionId = item.FunctionId;
                            functionRole.ActiveKey = item.ActiveKey;
                            functionRole.Type = (int)AppEnum.TypeFunction.FUNCTION_USER;
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

                    //update project
                    //foreach (var item in data.listProject)
                    //{
                    //    var exitsP = await db.UserMapping.Where(e => e.UserId == data.UserId && e.TargetId == item.ProjectId
                    //    && e.TargetType == (int)Const.TypeUserMap.USER_PROJECT).FirstOrDefaultAsync();
                    //    if (exitsP != null)
                    //    {
                    //        if (item.Check == true)
                    //        {
                    //            exitsP.Status = (int)Const.Status.NORMAL;
                    //        }
                    //        else
                    //        {
                    //            exitsP.Status = (int)Const.Status.DELETED;
                    //        }
                    //        db.UserMapping.Update(exitsP);
                    //    }
                    //    else
                    //    {
                    //        if (item.Check == true)
                    //        {
                    //            UserMapping userMapping = new UserMapping();
                    //            userMapping.UserId = data.UserId;
                    //            userMapping.TargetId = item.ProjectId;
                    //            userMapping.TargetType = (int)Const.TypeUserMap.USER_PROJECT;
                    //            userMapping.CreatedAt = DateTime.Now;
                    //            userMapping.UpdatedAt = DateTime.Now;
                    //            userMapping.UserIdCreatedId = userId;
                    //            userMapping.Status = (int)Const.Status.NORMAL;
                    //            await db.UserMapping.AddAsync(userMapping);
                    //        }
                    //    }

                    //    var listTower = data.listTower.Where(e => e.ProjectId == item.ProjectId).ToList();
                    //    //add tower
                    //    foreach (var itemT in listTower)
                    //    {
                    //        var exitsT = await db.UserMapping.Where(e => e.UserId == data.UserId && e.TargetId == itemT.TowerId
                    //        && e.TargetType == (int)Const.TypeUserMap.USER_TOWER).FirstOrDefaultAsync();
                    //        if (exitsT != null)
                    //        {
                    //            if (itemT.Check == true)
                    //            {
                    //                exitsT.Status = (int)Const.Status.NORMAL;
                    //            }
                    //            else
                    //            {
                    //                exitsT.Status = (int)Const.Status.DELETED;
                    //            }
                    //            db.UserMapping.Update(exitsT);
                    //        }
                    //        else
                    //        {
                    //            if (itemT.Check == true)
                    //            {
                    //                UserMapping userMappingT = new UserMapping();
                    //                userMappingT.UserId = data.UserId;
                    //                userMappingT.TargetId = itemT.TowerId;
                    //                userMappingT.TargetType = (int)Const.TypeUserMap.USER_TOWER;
                    //                userMappingT.CreatedAt = DateTime.Now;
                    //                userMappingT.UpdatedAt = DateTime.Now;
                    //                userMappingT.UserIdCreatedId = userId;
                    //                userMappingT.Status = (int)Const.Status.NORMAL;
                    //                await db.UserMapping.AddAsync(userMappingT);
                    //            }
                    //        }
                    //    }
                    //}
                    await _unitOfWork.CommitChangesAsync();
                    _unitOfWork.CommitTransaction();
                    //await db.SaveChangesAsync();

                    //transaction.Commit();
                    ////create action
                    //Models.EF.Action action = new Models.EF.Action();
                    //action.ActionId = Guid.NewGuid();
                    //action.ActionName = "Sửa tài khoản " + data.UserName;
                    //action.ActionType = "UPDATE";
                    //action.TargetId = data.UserId.ToString();
                    //action.TargetType = "User";
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
                    //return Ok(def);
                    return current;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_USERROLE_UPDATE_FAILED);
                }

            }
        }
    }
}

