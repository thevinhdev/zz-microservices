using AutoMapper;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Service;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Queries
{
    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    //public class LoginUserCmsQuery : IRequest<UserLogin>
    //{
    //    public string email { get; set; }
    //    public string password { get; set; }
    //}

    public record LoginUserCmsQuery(LoginRequest LoginRequest) : IRequest<UserLogin>;

    public class LoginUserCmsQueryHandler : IRequestHandler<LoginUserCmsQuery, UserLogin>
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserAsyncRepository _userRepo;
        private readonly IAsyncRepository<Employee> _empRepo;
        private readonly IUserRoleAsyncRepository _userRoleRepo;
        private readonly IFunctionRoleAsyncRepository _funcRoleRepo;
        private readonly IFunctionAsyncRepository _funcRepo;
        private readonly IAsyncRepository<Role> _roleRepo;

        public LoginUserCmsQueryHandler(IConfiguration configuration, IMapper mapper, IUserAsyncRepository userRepo, IAsyncRepository<Employee> empRepo,
            IUserRoleAsyncRepository userRoleRepo, IFunctionRoleAsyncRepository funcRoleRepo,
            IFunctionAsyncRepository funcRepo, IAsyncRepository<Role> roleRepo)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userRepo = userRepo;
            _empRepo = empRepo;
            _userRoleRepo = userRoleRepo;
            _funcRoleRepo = funcRoleRepo;
            _funcRepo = funcRepo;
            _roleRepo = roleRepo;
        }

        public async Task<UserLogin> Handle(LoginUserCmsQuery request1, CancellationToken cancellationToken)
        {
            var request = request1.LoginRequest;
            UserLogin userLogin = new UserLogin();
            var user = await _userRepo.FindByNameAsync(request.email, cancellationToken);
            //var user = db.User.Where(e => e.UserName == username
            //        && e.Type != (int)Const.TypeUser.RESIDENT_MAIN && e.Type != (int)Const.TypeUser.RESIDENT_GUEST
            //        && e.Status != (int)Const.Status.DELETED).ToList();
            if (user != null)
            {
                string password = user.KeyRandom.Trim() + user.RegEmail.Trim() + user.Id + request.password.Trim();
                password = Utils.GetMD5Hash(password);
                //UserLogin userLogin = db.User.Where(e => e.UserName == username
                //&& e.Password == password && e.Status != (int)Const.Status.DELETED
                //&& e.Type != (int)Const.TypeUser.RESIDENT_MAIN && e.Type != (int)Const.TypeUser.RESIDENT_GUEST).Select(e => new UserLogin()
                //{
                //    userId = e.UserId,
                //    userMapId = e.UserMapId,
                //    userName = e.UserName,
                //    email = e.Email,
                //    fullName = e.FullName,
                //    avata = e.Avata,
                //    address = e.Address,
                //    password = e.Password,
                //    phone = e.Phone,
                //    roleMax = e.RoleMax,
                //    roleLevel = e.RoleLevel,
                //    isRoleGroup = e.IsRoleGroup != null ? (bool)e.IsRoleGroup : true,
                //    type = e.Type,
                //    status = e.Status,
                //}).FirstOrDefault();
                var userL = await _userRepo.CheckUserLoginAsync(request.email, password, cancellationToken);
                if (userL != null)
                {
                    userLogin.userId = userL.Id;
                    userLogin.userMapId = userL.UserMapId;
                    userLogin.userName = userL.UserName;
                    userLogin.email = userL.Email;
                    userLogin.fullName = userL.FullName;
                    userLogin.avata = userL.Avata;
                    userLogin.address = userL.Address;
                    userLogin.password = userL.Password;
                    userLogin.phone = userL.Phone;
                    userLogin.roleMax = userL.RoleMax;
                    userLogin.roleLevel = userL.RoleLevel;
                    userLogin.isRoleGroup = userL.IsRoleGroup != null ? (bool)userL.IsRoleGroup : true;
                    userLogin.type = userL.Type;
                    userLogin.status = (int)userL.Status;

                    if (userLogin != null)
                    {
                        //check if user lock
                        if (userLogin.status == (int)AppEnum.EntityStatus.LOCK)
                        {
                            //def.meta = new Meta(223, "Tài khoản đã bị khóa!");
                            //return Ok(def);
                            throw new CommonException("Tài khoản đã bị khóa!", 223, ApiConstants.ErrorCode.ERROR_USER_LOCKED);
                        }

                        if (userLogin.type == (int)AppEnum.TypeUser.MANAGEMENT || userLogin.type == (int)AppEnum.TypeUser.TECHNICIANS)
                        {
                            //var employee = await db.Employee.Where(e => e.EmployeeId == userLogin.userMapId).FirstOrDefaultAsync();
                            var employee = await _empRepo.GetByKeyAsync((int)userLogin.userMapId);
                            if (employee != null)
                            {
                                userLogin.departmentId = employee.DepartmentId;
                                userLogin.positionId = employee.PositionId;
                                userLogin.projectId = employee.ProjectId;
                                userLogin.fullName = employee.FullName;
                                userLogin.email = employee.Email;
                                userLogin.phone = employee.Phone;
                                userLogin.address = employee.Address;
                                userLogin.avata = employee.Avata;
                            }
                        }

                        var userId = userLogin.userId;
                        List<MenuDTO> listFunctionRole = new List<MenuDTO>();
                        //lấy danh sách quyền theo chức năng, nếu danh sách quyền theo chức năng null thì lấy
                        //danh sách quyền theo nhóm quyền

                        if (!userLogin.isRoleGroup)
                        {
                            var listFR = _funcRoleRepo.All().Where(e => e.TargetId == userId && e.Type == (int)AppEnum.TypeFunction.FUNCTION_USER
                            && e.Status == AppEnum.EntityStatus.NORMAL).OrderBy(e => e.Function.Location).ToList();
                            //var listFR = await _funcRoleRepo.GetListFunctionRoleAsync((long)userId, (int)AppEnum.TypeFunction.FUNCTION_USER, cancellationToken);
                            foreach (var itemFR in listFR)
                            {
                                //check exits
                                var fr = listFunctionRole.Where(e => e.MenuId == itemFR.FunctionId).ToList();
                                if (fr.Count > 0)
                                {
                                    string key1 = fr.FirstOrDefault().ActiveKey;
                                    if (fr.FirstOrDefault().ActiveKey != itemFR.ActiveKey)
                                    {
                                        key1 = UserService.PlusActiveKey(fr.FirstOrDefault().ActiveKey, itemFR.ActiveKey);
                                    }
                                    fr.FirstOrDefault().ActiveKey = key1;
                                }
                                else
                                {
                                    MenuDTO menu = new MenuDTO();
                                    menu.MenuId = itemFR.FunctionId;
                                    menu.Code = itemFR.Function.Code;
                                    menu.Name = itemFR.Function.Name;
                                    menu.Url = itemFR.Function.Url;
                                    menu.Icon = itemFR.Function.Icon;
                                    menu.MenuParent = (int)itemFR.Function.FunctionParentId;
                                    menu.ActiveKey = itemFR.ActiveKey;
                                    listFunctionRole.Add(menu);
                                }
                            }
                        }
                        else
                        {
                            //Lấy all funcation
                            var listF = await _funcRepo.GetListAllFunctionAsync(cancellationToken);
                            //get list user role
                            //var userRole = db.UserRole.Where(e => e.UserId == userId && e.Status == (int)Const.Status.NORMAL).ToList();
                            //var spec = new UserRoleFilterWithPagingSpec(request);
                            var userRole = await _userRoleRepo.GetListUserRoleAsync(userId, cancellationToken);
                            //get list function role
                            foreach (var item in userRole)
                            {
                                var listFRR = _funcRoleRepo.All().Where(e => e.TargetId == item.RoleId && e.Type == (int)AppEnum.TypeFunction.FUNCTION_ROLE
                                    && e.Status == AppEnum.EntityStatus.NORMAL).OrderBy(e => e.Function.Location).ToList();
                                //var listFRR = await _funcRoleRepo.GetListFunctionRoleAsync((long)item.RoleId, (int)AppEnum.TypeFunction.FUNCTION_ROLE, cancellationToken);
                                foreach (var itemFR in listFRR)
                                {
                                    //check exits
                                    var fr = listFunctionRole.Where(e => e.MenuId == itemFR.FunctionId).ToList();
                                    if (fr.Count > 0)
                                    {
                                        string key1 = fr.FirstOrDefault().ActiveKey;
                                        if (fr.FirstOrDefault().ActiveKey != itemFR.ActiveKey)
                                        {
                                            key1 = UserService.PlusActiveKey(fr.FirstOrDefault().ActiveKey, itemFR.ActiveKey);
                                        }
                                        fr.FirstOrDefault().ActiveKey = key1;
                                    }
                                    else
                                    {
                                        Function function = listF.Where(e => e.Id == itemFR.FunctionId && e.Status == AppEnum.EntityStatus.NORMAL).FirstOrDefault();
                                        if (function != null)
                                        {
                                            MenuDTO menu = new MenuDTO();
                                            menu.MenuId = itemFR.FunctionId;
                                            menu.Code = function.Code;
                                            menu.Name = function.Name;
                                            menu.Url = function.Url;
                                            menu.Icon = function.Icon;
                                            menu.MenuParent = (int)function.FunctionParentId;
                                            menu.ActiveKey = itemFR.ActiveKey;
                                            listFunctionRole.Add(menu);
                                        }
                                    }
                                }
                            }
                            if (userRole != null)
                            {
                                //var role = await db.Role.Where(e => e.RoleId == userRole.FirstOrDefault().RoleId).FirstOrDefaultAsync();
                                var role = await _roleRepo.GetByKeyAsync((int)userRole.FirstOrDefault().RoleId);
                                if (role != null)
                                {
                                    userLogin.roleCode = role.Code;
                                }
                            }
                        }

                        string access_key = "";
                        int count = listFunctionRole.Count;
                        if (count > 0)
                        {
                            for (int i = 0; i < count - 1; i++)
                            {
                                if (listFunctionRole[i].ActiveKey != "000000000")
                                {
                                    access_key += listFunctionRole[i].Code + ":" + listFunctionRole[i].ActiveKey + "-";
                                }
                            }

                            access_key = access_key + listFunctionRole[count - 1].Code + ":" + listFunctionRole[count - 1].ActiveKey;
                        }

                        userLogin.access_key = access_key;
                        userLogin.listMenus = UserService.CreateMenu(listFunctionRole, 0);

                        //
                        //check nếu có 1 ngôn ngữ thì lấy ngôn ngữ mạc định là tiếng việt
                        int languageId = 1;
                        //var language = db.Language.Where(e => e.CompanyId == userLogin.companyId).ToList();
                        //if (language.Count == 1)
                        //{
                        //    languageId = language.FirstOrDefault().LanguageId;
                        //}
                        //else if (language.Count > 1)
                        //{
                        //    languageId = language.Where(e => e.IsMain == true).FirstOrDefault().LanguageId;
                        //}
                        userLogin.languageId = languageId;
                        userLogin.email = userLogin.email != null ? userLogin.email : "";
                        var claims = new List<Claim>
                                {
                                    new Claim(JwtRegisteredClaimNames.Email, userLogin.email),
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, userLogin.userId.ToString()),
                                    new Claim(ClaimTypes.Name, userLogin.fullName),
                                    new Claim(ClaimTypes.Email, userLogin.email != null ? userLogin.email : ""),
                                    new Claim(ClaimTypes.MobilePhone, userLogin.phone != null ? userLogin.phone : ""),
                                    new Claim("id", userLogin.userId.ToString()),
                                    new Claim("fullname", userLogin.fullName),
                                    new Claim("UserId", userLogin.userId.ToString()),
                                    new Claim("UserMapId", userLogin.userMapId != null ? userLogin.userMapId.ToString() : "-1"),
                                    new Claim("RoleMax", userLogin.roleMax != null ? userLogin.roleMax.ToString() : ""),
                                    new Claim("RoleLevel", userLogin.roleLevel != null ? userLogin.roleLevel.ToString() : ""),
                                    new Claim("RoleCode", userLogin.roleCode != null ? userLogin.roleCode.ToString() : ""),
                                    new Claim("AccessKey", access_key != null ? access_key : ""),
                                    new Claim("LanguageId", access_key != null ? languageId.ToString() : ""),
                                    new Claim("ProjectId", userLogin.projectId != null ? userLogin.projectId.ToString() : "0"),
                                    new Claim("Type", userLogin.type != null ? userLogin.type.ToString() : "-1"),
                                    new Claim("DepartmentId", userLogin.departmentId != null ? userLogin.departmentId.ToString() : ""),
                                    new Claim("PositionId", userLogin.positionId != null ? userLogin.positionId.ToString() : ""),
                                };

                        string JwtKey = _configuration["AppSettings:JwtKey"];
                        string JwtExpireDays = _configuration["AppSettings:JwtExpireDays"];
                        string JwtIssuer = _configuration["AppSettings:JwtIssuer"];
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var expires = DateTime.Now.AddDays(Convert.ToDouble(JwtExpireDays));

                        var token = new JwtSecurityToken(
                            JwtIssuer,
                            JwtIssuer,
                            claims,
                            expires: expires,
                            signingCredentials: creds
                        );

                        //update firestore
                        //try
                        //{
                        //    await SBOApi.Models.Data.Firebase.UpdateInfoUser(userLogin.userId, db);
                        //}
                        //catch { }

                        userLogin.access_token = new JwtSecurityTokenHandler().WriteToken(token);
                        //def.data = userLogin;
                        //def.meta = new Meta(200, "Đăng nhập thành công!");
                        //return Ok(def);
                        return _mapper.Map<UserLogin>(userLogin);
                    }
                    else
                    {
                        //check if email exist
                        //var existed = db.User.Where(e => e.UserName == username && e.Status != (int)Const.Status.DELETED).FirstOrDefault();
                        var existed = await _userRepo.FindByNameAsync(request.email, cancellationToken);
                        if (existed != null)
                        {
                            //def.meta = new Meta(213, "Tài khoản hoặc mật khẩu không chính xác!");
                            //return Ok(def);
                            throw new CommonException("Tài khoản hoặc mật khẩu không chính xác!", 213, ApiConstants.ErrorCode.ERROR_USER_LOGIN_NOT_FOUND);
                        }
                        else
                        {
                            //def.meta = new Meta(404, "Tài khoản hoặc mật khẩu không chính xác!");
                            //return Ok(def);
                            throw new CommonException("Tài khoản hoặc mật khẩu không chính xác!", 404, ApiConstants.ErrorCode.ERROR_USER_LOGIN_NOT_FOUND);
                        }
                    }
                }
                else
                {
                    throw new CommonException("Tài khoản hoặc mật khẩu không chính xác!", 404, ApiConstants.ErrorCode.ERROR_USER_LOGIN_NOT_FOUND);
                }
            }
            else
            {
                throw new CommonException("Tài khoản hoặc mật khẩu không chính xác!", 404, ApiConstants.ErrorCode.ERROR_USER_LOGIN_NOT_FOUND);
                //def.meta = new Meta(404, "Tài khoản hoặc mật khẩu không chính xác!");
                //return Ok(def);
            }
        }
    }
}
