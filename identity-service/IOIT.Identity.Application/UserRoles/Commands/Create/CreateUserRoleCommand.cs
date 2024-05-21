using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.UserRoles.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.UserRoles.Commands.Create
{
    public class CreateUserRoleCommand : IRequest<User>
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

        public class Validation : AbstractValidator<CreateUserRoleCommand>
        {
            public Validation()
            {
                //RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName not empty");
            }
        }

        public class Handler : IRequestHandler<CreateUserRoleCommand, User>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _entityRepository;
            private readonly IEmployeeAsyncRepository _empRepo;
            private readonly IResidentAsyncRepository _residentRepo;
            private readonly IRoleAsyncRepository _roleRepo;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository entityRepository,
                IEmployeeAsyncRepository empRepo,
                IResidentAsyncRepository residentRepo,
                IRoleAsyncRepository roleRepo,
                IFunctionRoleAsyncRepository funcRoleRepo,
                IUserRoleAsyncRepository userRoleRepo,
                IPublishEndpoint publishEndpoint)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _empRepo = empRepo;
                _residentRepo = residentRepo;
                _roleRepo = roleRepo;
                _funcRoleRepo = funcRoleRepo;
                _userRoleRepo = userRoleRepo;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<User> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
            {
                if (request.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || request.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                {
                    //User checkUserNameExist = db.User.Where(f => Utils.CheckPhone(data.UserName, f.UserName)
                    //&& (f.Type == (int)Const.TypeUser.RESIDENT_MAIN || f.Type == (int)Const.TypeUser.RESIDENT_GUEST)
                    //&& f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                    var checkUserNameExist = await _entityRepository.FindByPhoneUsernameAsync(request.UserName, 1, cancellationToken);
                    if (checkUserNameExist != null)
                    {
                        //def.meta = new Meta(211, "Tài khoản đã tồn tại!");
                        //return Ok(def);
                        throw new CommonException("Tài khoản đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_USERNAME_EXISTED);
                    }

                    //User checkUserIdExist = db.User.Where(f => f.UserMapId == data.UserMapId
                    //&& (f.Type == (int)Const.TypeUser.RESIDENT_MAIN || f.Type == (int)Const.TypeUser.RESIDENT_GUEST)
                    //&& f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                    var checkUserIdExist = await _entityRepository.FindByUserMapIdAsync(request.UserMapId, 1, cancellationToken);
                    if (checkUserIdExist != null)
                    {
                        //def.meta = new Meta(211, "Tài khoản đã được tạo với số điện thoại:" + checkUserIdExist.UserName);
                        //return Ok(def);
                        throw new CommonException("Tài khoản đã được tạo với số điện thoại:" + checkUserIdExist.UserName, 211, ApiConstants.ErrorCode.ERROR_PHONE_EXISTED);
                    }
                }
                else if (request.Type == (int)AppEnum.TypeUser.MANAGEMENT || request.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                {
                    //User checkUserNameExist = db.User.Where(f => Utils.CheckPhone(data.UserName, f.UserName)
                    //&& (f.Type == (int)Const.TypeUser.MANAGEMENT || f.Type == (int)Const.TypeUser.TECHNICIANS)
                    //&& f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                    var checkUserNameExist = await _entityRepository.FindByPhoneUsernameAsync(request.UserName, 2, cancellationToken);
                    if (checkUserNameExist != null)
                    {
                        throw new CommonException("Tài khoản đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_USERNAME_EXISTED);
                    }

                    //User checkUserIdExist = db.User.Where(f => f.UserMapId == data.UserMapId
                    //&& (f.Type == (int)Const.TypeUser.MANAGEMENT || f.Type == (int)Const.TypeUser.TECHNICIANS)
                    //&& f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                    var checkUserIdExist = await _entityRepository.FindByUserMapIdAsync(request.UserMapId, 2, cancellationToken);
                    if (checkUserIdExist != null)
                    {
                        //def.meta = new Meta(211, "Tài khoản đã được tạo với số điện thoại:" + checkUserIdExist.UserName);
                        //return Ok(def);
                        throw new CommonException("Tài khoản đã được tạo với số điện thoại:" + checkUserIdExist.UserName, 211, ApiConstants.ErrorCode.ERROR_PHONE_EXISTED);
                    }
                }
                else
                {
                    //User checkUserNameExist = db.User.Where(f => f.UserName == data.UserName
                    //&& (f.Type == (int)Const.TypeUser.ADMINISTRATOR)
                    //&& f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                    var checkUserNameExist = await _entityRepository.FindByPhoneUsernameAsync(request.UserName, 3, cancellationToken);
                    if (checkUserNameExist != null)
                    {
                        throw new CommonException("Tài khoản đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_USERNAME_EXISTED);
                    }

                    if (request.Email != null && request.Email != "")
                    {
                        //User checkEmailExist = db.User.Where(f => f.Email == data.Email
                        //&& (f.Type == (int)Const.TypeUser.ADMINISTRATOR)
                        //&& f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                        var checkEmailExist = await _entityRepository.FindByEmailAsync(request.Email, 0, cancellationToken);
                        if (checkEmailExist != null)
                        {
                            //def.meta = new Meta(2111, "Email đã tồn tại!");
                            //return Ok(def);
                            throw new CommonException("Email đã tồn tại!", 211, ApiConstants.ErrorCode.ERROR_EMAIL_EXISTED);
                        }
                    }
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                //check xem có tạo nhân viên mới ko
                if (request.UserMapId != null)
                {
                    //check xem id nhân viên có chuẩn ko
                    if (request.Type == (int)AppEnum.TypeUser.MANAGEMENT || request.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                    {
                        //var emp = await db.Employee.Where(e => e.EmployeeId == data.UserMapId).FirstOrDefaultAsync();
                        var emp = await _empRepo.FindByUserMapIdAsync(request.UserMapId, cancellationToken);
                        if (emp == null)
                        {
                            //def.meta = new Meta(2112, "Nhân viên tạo tài khoản không tồn tại!");
                            //return Ok(def);
                            throw new CommonException("Nhân viên tạo tài khoản không tồn tại!", 212, ApiConstants.ErrorCode.ERROR_USERRESIDENT_NOT_EXIST);
                        }
                    }
                    else if (request.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || request.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                    {
                        //var res = await db.Resident.Where(e => e.ResidentId == data.UserMapId).FirstOrDefaultAsync();
                        var res = await _residentRepo.CheckUserMapIdAsync(request.UserMapId, cancellationToken);
                        if (res == null)
                        {
                            //def.meta = new Meta(2112, "Cư dân tạo tài khoản không tồn tại!");
                            //return Ok(def);
                            throw new CommonException("Cư dân tạo tài khoản không tồn tại!", 212, ApiConstants.ErrorCode.ERROR_USERRESIDENT_NOT_EXIST);
                        }
                    }
                }
                else
                {
                    //Tạo nhanh nhân viên
                    if (request.Type == (int)AppEnum.TypeUser.MANAGEMENT || request.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                    {
                        Employee employee = new Employee();
                        employee.FullName = request.FullName;
                        employee.Code = request.Code;
                        employee.Avata = request.Avata;
                        //var project = data.listProject.Where(e => e.Check == true).FirstOrDefault();
                        //if (project != null)
                        employee.ProjectId = request.ProjectId;
                        //elss
                        employee.DepartmentId = request.DepartmentId;
                        employee.PositionId = request.PositionId;
                        employee.Birthday = request.Birthday;
                        employee.CardId = request.CardId;
                        employee.Phone = request.Phone;
                        employee.Email = request.Email;
                        employee.Address = request.Address;
                        employee.Note = request.Note;
                        employee.TypeEmployee = request.Type == (int)AppEnum.TypeUser.MANAGEMENT ? (byte)AppEnum.TypeEmployee.MANAGER : (byte)AppEnum.TypeEmployee.USER;
                        employee.CreatedAt = DateTime.Now;
                        employee.UpdatedAt = DateTime.Now;
                        employee.CreatedById = request.UserId;
                        employee.UpdatedById = request.UserId;
                        employee.Status = AppEnum.EntityStatus.NORMAL;
                        //await db.Employee.AddAsync(employee);
                        //await db.SaveChangesAsync();
                        await _empRepo.AddAsync(employee);
                        await _unitOfWork.CommitChangesAsync();
                        request.UserMapId = employee.Id;

                        //Gọi Producers để thêm vào các service khác
                        var messageEmployee = _mapper.Map<DtoCommonEmployeeQueue>(employee);
                        await _publishEndpoint.Publish<DtoCommonEmployeeQueue>(messageEmployee);
                    }
                }

                User user = new User();
                user.Address = request.Address;
                user.FullName = request.FullName;
                user.UserName = request.UserName;
                user.CardId = request.CardId;
                user.UserMapId = request.UserMapId;
                user.TypeThird = request.TypeThird;
                user.Type = request.Type;
                user.Code = request.Code;
                user.Email = request.Email;
                user.Avata = request.Avata;
                user.Password = Utils.GetMD5Hash(request.Password);
                user.Phone = request.Phone;
                user.DepartmentId = request.DepartmentId;
                user.PositionId = request.PositionId;
                user.KeyRandom = Utils.RandomString(20);
                user.RegEmail = Utils.RandomString(8);
                user.RoleMax = 9999;
                user.RoleLevel = 99;
                user.IsRoleGroup = request.IsRoleGroup != null ? request.IsRoleGroup : true;
                user.IsPhoneConfirm = request.IsPhoneConfirm;
                user.IsEmailConfirm = request.IsEmailConfirm;
                user.CountLogin = 0;
                user.LanguageId = 1;
                user.ProjectId = request.ProjectId;
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                user.LastLoginAt = DateTime.Now;
                user.Status = AppEnum.EntityStatus.NORMAL;
                user.CreatedById = request.UserId;
                user.UpdatedById = request.UserId;
                //await db.User.AddAsync(user);
                //await db.SaveChangesAsync();
                await _entityRepository.AddAsync(user);
                await _unitOfWork.CommitChangesAsync();
                request.Id = user.Id;

                //Gọi Producers để thêm vào các service khác
                var messageUser = _mapper.Map<DtoCommonUserQueue>(user);
                await _publishEndpoint.Publish<DtoCommonUserQueue>(messageUser);

                //update pass
                string pass = user.KeyRandom.Trim() + user.RegEmail.Trim() + user.Id + user.Password.Trim();
                user.Password = Utils.GetMD5Hash(pass);

                if (request.Type != (int)AppEnum.TypeUser.RESIDENT_MAIN && request.Type != (int)AppEnum.TypeUser.RESIDENT_GUEST)
                {
                    // role
                    var checkRole = false;
                    byte level = 99;
                    int max = 9999;
                    //int type = user.Type != null ? (int)user.Type : 5;
                    //add role 
                    foreach (var item in request.listRole)
                    {
                        //var role = db.Role.Find(item.RoleId);
                        var role = await _roleRepo.GetByKeyAsync(item.RoleId);
                        if (role != null)
                        {
                            UserRole userRole = new UserRole();
                            userRole.RoleId = item.RoleId;
                            userRole.UserId = user.Id;
                            userRole.CreatedAt = DateTime.Now;
                            userRole.UpdatedAt = DateTime.Now;
                            userRole.CreatedById = request.UserId;
                            userRole.UpdatedById = request.UserId;
                            userRole.Status = AppEnum.EntityStatus.NORMAL;
                            //db.UserRole.Add(userRole);
                            await _userRoleRepo.AddAsync(userRole);
                            //check role
                            if (role.Code.Trim() == "ADMIN" || role.Code.Trim() == "MANAGER" || role.Code.Trim() == "USER" || role.Code.Trim() == "MANAGER_FULL")
                                checkRole = true;
                            //
                            if (role.LevelRole < level)
                            {
                                level = (byte)role.LevelRole;
                                max = role.Id;
                                //if (role.Code.Trim() == "ADMIN") type = 1;
                                //else if (role.Code.Trim() == "MANAGER") type = 2;
                                //else if (role.Code.Trim() == "USER") type = 3;
                            }
                        }
                    }
                    //update cấp độ user và quyền cao nhất của user đó
                    //if (user.Type != (int)Const.TypeUser.RESIDENT_MAIN && user.Type != (int)Const.TypeUser.RESIDENT_GUEST)
                    //{
                    //    user.Type = (byte)type;
                    //}
                    user.RoleLevel = level;
                    user.RoleMax = max;
                    //db.Entry(user).State = EntityState.Modified;
                    _entityRepository.Update(user);

                    //add function
                    foreach (var item in request.listFunction)
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
                        //await db.FunctionRole.AddAsync(functionRole);
                        await _funcRoleRepo.AddAsync(functionRole);
                    }

                    //add project
                    //foreach (var item in data.listProject)
                    //{
                    //    if (item.Check == true)
                    //    {
                    //        UserMapping userMapping = new UserMapping();
                    //        userMapping.UserId = user.UserId;
                    //        userMapping.TargetId = item.ProjectId;
                    //        userMapping.TargetType = (int)Const.TypeUserMap.USER_PROJECT;
                    //        userMapping.CreatedAt = DateTime.Now;
                    //        userMapping.UpdatedAt = DateTime.Now;
                    //        userMapping.UserIdCreatedId = userId;
                    //        userMapping.Status = (int)Const.Status.NORMAL;
                    //        await db.UserMapping.AddAsync(userMapping);

                    //        var listTower = data.listTower.Where(e => e.ProjectId == item.ProjectId && e.Check == true).ToList();
                    //        //add tower
                    //        foreach (var itemT in listTower)
                    //        {
                    //            UserMapping userMappingT = new UserMapping();
                    //            userMappingT.UserId = user.UserId;
                    //            userMappingT.TargetId = itemT.TowerId;
                    //            userMappingT.TargetType = (int)Const.TypeUserMap.USER_TOWER;
                    //            userMappingT.CreatedAt = DateTime.Now;
                    //            userMappingT.UpdatedAt = DateTime.Now;
                    //            userMappingT.UserIdCreatedId = userId;
                    //            userMappingT.Status = (int)Const.Status.NORMAL;
                    //            await db.UserMapping.AddAsync(userMappingT);
                    //        }
                    //    }
                    //}
                }

                try
                {
                    //await db.SaveChangesAsync();
                    await _unitOfWork.CommitChangesAsync();
                    if (user.Id > 0)
                    {
                        _unitOfWork.CommitTransaction();
                        //transaction.Commit();
                        ////create action
                        //Models.EF.Action action = new Models.EF.Action();
                        //action.ActionId = Guid.NewGuid();
                        //action.ActionName = "Tạo tài khoản " + data.UserName;
                        //action.ActionType = "CREATE";
                        //action.TargetId = data.UserId.ToString(); ;
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

                        //def.meta = new Meta(200, Const.CREATE_SUCCESS_MESSAGE);
                        //def.data = data;
                        //return Ok(def);
                        return user;
                    }
                    else
                    {
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_USERROLE_CREATE_FAILED);
                    }
                }
                catch (DbUpdateException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_USERROLE_CREATE_FAILED);
                }
                //}
                //var entity = _mapper.Map<UserRole>(request);

                //await _entityRepository.AddAsync(entity);
                //await _unitOfWork.CommitChangesAsync();

                //return entity;
            }
        }
    }
}
