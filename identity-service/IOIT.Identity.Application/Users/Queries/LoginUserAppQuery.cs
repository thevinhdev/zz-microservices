using AutoMapper;
using Automatonymous;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Service;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using IOIT.Shared.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Users.Queries
{
    public class LoginUserAppQuery : IRequest<ResUserLoginApp>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int TypeLanguage { get; set; }
        public int TypeUser { get; set; }
        //public string JwtKey { get; set; }
        //public string JwtExpireDays { get; set; }
        //public string JwtIssuer { get; set; }

        public class Validation : AbstractValidator<LoginUserAppQuery>
        {
            public Validation()
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage("Username not empty");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Password not empty");
            }
        }

        public class LoginUserAppHandler : IRequestHandler<LoginUserAppQuery, ResUserLoginApp>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IConfiguration _configuration;
            private readonly IUserAsyncRepository _userRepo;
            private readonly ILogger<LoginUserAppHandler> _logger;
            private readonly IAsyncRepository<Employee> _empRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;
            private readonly IFunctionAsyncRepository _funcRepo;
            private readonly IAsyncRepository<Role> _roleRepo;
            private readonly IAsyncLongRepository<Resident> _residentRepo;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly IProjectAsyncRepository _projectRepo;
            private readonly IDepartmentAsyncRepository _departmentRepo;
            private readonly ITypeAttributeItemAsyncRepository _typeAttributeItem;
            private IHttpClientFactory _factory;

            public LoginUserAppHandler(IMapper mapper, IConfiguration configuration,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo, IAsyncRepository<Employee> empRepo,
                ILogger<LoginUserAppHandler> logger,
                IUserRoleAsyncRepository userRoleRepo, IFunctionRoleAsyncRepository funcRoleRepo,
                IFunctionAsyncRepository funcRepo, IAsyncRepository<Role> roleRepo,
                IAsyncLongRepository<Resident> residentRepo, IApartmentMapAsyncRepository amRepo,
                IProjectAsyncRepository projectRepo, IDepartmentAsyncRepository departmentRepo,
                ITypeAttributeItemAsyncRepository typeAttributeItem,
                IHttpClientFactory factory
                )
            {
                _mapper = mapper;
                _configuration = configuration;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _empRepo = empRepo;
                _userRoleRepo = userRoleRepo;
                _logger = logger;
                _funcRoleRepo = funcRoleRepo;
                _funcRepo = funcRepo;
                _roleRepo = roleRepo;
                _residentRepo = residentRepo;
                _amRepo = amRepo;
                _projectRepo = projectRepo;
                _departmentRepo = departmentRepo;
                _typeAttributeItem = typeAttributeItem;
                _factory = factory;
            }

            public async Task<ResUserLoginApp> Handle(LoginUserAppQuery request, CancellationToken cancellationToken)
            {
                string Username = request.Username;
                ResUserLoginApp userLogin = new ResUserLoginApp();
                userLogin.MetaCode = 200;
                userLogin.MetaMess = ApiConstants.MessageResource.ACCTION_SUCCESS;

                var user = new User();
                if (request.TypeUser == (int)AppEnum.TypeUser.RESIDENT_MAIN || request.TypeUser == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                {
                    string userName2 = Utils.ConvertPhone(Username);
                    user = _userRepo.All().AsNoTracking().Where(e => (e.UserName.Trim() == Username || e.UserName.Trim() == userName2)
                    && e.Status != AppEnum.EntityStatus.DELETED
                    && (e.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || e.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST)).FirstOrDefault();
                }
                else if (request.TypeUser == (int)AppEnum.TypeUser.MANAGEMENT || request.TypeUser == (int)AppEnum.TypeUser.TECHNICIANS)
                {
                    user = _userRepo.All().AsNoTracking().Where(e => e.UserName == Username && e.Status != AppEnum.EntityStatus.DELETED
                    && (e.Type == (int)AppEnum.TypeUser.MANAGEMENT || e.Type == (int)AppEnum.TypeUser.TECHNICIANS)).FirstOrDefault();
                }
                if (user != null)
                {
                    if (user.IsDeletedByGuest == true)
                    {
                        throw new CommonException(Resources.USER_NOT_FOUND, 404, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                    }
                    //string password = user.FirstOrDefault().KeyRandom.Trim() + user.FirstOrDefault().RegEmail.Trim() + user.FirstOrDefault().UserId + loginModel.password.Trim();
                    string password = user.KeyRandom + user.RegEmail + user.Id + request.Password.Trim();
                    password = Utils.GetMD5Hash(password);
                    //UserLogin userLogin = new UserLogin();
                    if (request.TypeUser == (int)AppEnum.TypeUser.RESIDENT_MAIN || request.TypeUser == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                    {
                        string phone2 = Utils.ConvertPhone(request.Username);
                        //userLogin = _userRepo.All().Where(e => (e.UserName == request.Username || e.UserName == phone2)
                        //    && e.Password == password && e.Status != AppEnum.EntityStatus.DELETED
                        //    && (e.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || e.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST)).Select(e => new UserLogin()
                        //    {
                        //        UserId = e.Id,
                        //        UserMapId = e.UserMapId,
                        //        DepartmentId = e.DepartmentId,
                        //        positionId = e.PositionId,
                        //        UserName = e.UserName,
                        //        LanguageId = e.LanguageId,
                        //        Type = e.Type,
                        //        CountLogin = e.CountLogin,
                        //        Status = e.Status,
                        //        IsPhoneConfirm = e.IsPhoneConfirm,
                        //        RegisterCode = e.RegisterCode,
                        //    }).FirstOrDefault();
                        var userL = await _userRepo.CheckUserLoginAppAsync(request.Username, password, 1);
                        if (userL != null)
                        {
                            userLogin.UserId = userL.Id;
                            userLogin.UserMapId = userL.UserMapId;
                            userLogin.DepartmentId = userL.DepartmentId;
                            userLogin.PositionId = userL.PositionId;
                            userLogin.UserName = userL.UserName;
                            userLogin.LanguageId = userL.LanguageId;
                            userLogin.Type = userL.Type;
                            userLogin.CountLogin = userL.CountLogin;
                            userLogin.Status = (byte)userL.Status;
                            userLogin.IsPhoneConfirm = userL.IsPhoneConfirm;
                            userLogin.RegisterCode = userL.RegisterCode;

                            //if (userLogin != null)
                            //{
                            //var resident = await db.Resident.Where(e => e.ResidentId == userLogin.UserMapId).FirstOrDefaultAsync();
                            var resident = await _residentRepo.GetByKeyAsync((long)userLogin.UserMapId);
                            if (resident != null)
                            {
                                userLogin.FullName = resident.FullName;
                                userLogin.Sex = resident.Sex;
                                userLogin.Email = resident.Email;
                                userLogin.Phone = resident.Phone;
                                userLogin.Address = resident.Address;
                                userLogin.CardId = resident.CardId;
                                userLogin.Avata = (resident.Avata != null && resident.Avata != "") ? resident.Avata : AppEnum.DEFAULT_AVATAR;
                                userLogin.Birthday = resident.Birthday;
                                userLogin.ProjectId = -1;
                                //Trả về 1 projectid default
                                //List<ApartmentMap> apartmentMaps = db.ApartmentMap.Where(ap => ap.ResidentId == resident.ResidentId && ap.Status != (int)Const.Status.DELETED).ToList();
                                var apartmentMaps = await _amRepo.GetListByResidentAsync(resident.Id, cancellationToken);
                                ApartmentMap CHU_NHA = apartmentMaps.Where(x => x.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN).FirstOrDefault();
                                if (CHU_NHA != null)
                                {
                                    userLogin.ProjectId = CHU_NHA.ProjectId;
                                    userLogin.DepartmentId = CHU_NHA.ApartmentId;
                                    userLogin.Type = 4;
                                }
                                else
                                {
                                    ApartmentMap THANH_VIEN_CH = apartmentMaps.Where(x => x.Type == (int)AppEnum.TypeResident.RESIDENT_MEMBER).FirstOrDefault();
                                    if (THANH_VIEN_CH != null)
                                    {
                                        userLogin.ProjectId = THANH_VIEN_CH.ProjectId;
                                        userLogin.DepartmentId = THANH_VIEN_CH.ApartmentId;
                                        userLogin.Type = 7;
                                    }
                                    else
                                    {
                                        ApartmentMap KHACH_THUE = apartmentMaps.Where(x => x.Type == (int)AppEnum.TypeResident.RESIDENT_GUEST).FirstOrDefault();
                                        if (KHACH_THUE != null)
                                        {
                                            userLogin.ProjectId = KHACH_THUE.ProjectId;
                                            userLogin.DepartmentId = KHACH_THUE.ApartmentId;
                                            userLogin.Type = 5;
                                        }
                                        else
                                        {
                                            ApartmentMap THANHVIEN_KT = apartmentMaps.Where(x => x.Type == (int)AppEnum.TypeResident.RESIDENT_GUEST_MEMBER).FirstOrDefault();
                                            if (THANHVIEN_KT != null)
                                            {
                                                userLogin.ProjectId = THANHVIEN_KT.ProjectId;
                                                userLogin.DepartmentId = THANHVIEN_KT.ApartmentId;
                                                userLogin.Type = 8;
                                            }
                                            else
                                            {
                                                ApartmentMap KHAC = apartmentMaps.FirstOrDefault();
                                                if (KHAC != null)
                                                {
                                                    userLogin.ProjectId = KHAC.ProjectId;
                                                    userLogin.DepartmentId = KHAC.ApartmentId;
                                                    userLogin.Type = 9;
                                                }
                                            }
                                        }
                                    }

                                }

                            }
                        }
                        else
                        {
                            userLogin = null;
                        }
                    }
                    else
                    {

                        var userL = await _userRepo.CheckUserLoginAppAsync(request.Username, password, 2);
                        if (userL != null)
                        {
                            userLogin.UserId = userL.Id;
                            userLogin.UserMapId = userL.UserMapId;
                            userLogin.UserName = userL.UserName;
                            userLogin.LanguageId = userL.LanguageId;
                            userLogin.Type = userL.Type;
                            userLogin.CountLogin = userL.CountLogin;
                            userLogin.Status = (byte)userL.Status;
                            userLogin.IsPhoneConfirm = userL.IsPhoneConfirm;
                            userLogin.RegisterCode = userL.RegisterCode;
                            userLogin.IsRoleGroup = userL.IsRoleGroup;

                            //if (userLogin != null)
                            //{
                            var employee = await _empRepo.GetByKeyAsync((int)userLogin.UserMapId);
                            //var employee = await db.Employee.Where(e => e.EmployeeId == userLogin.UserMapId).FirstOrDefaultAsync();
                            if (employee != null)
                            {
                                userLogin.DepartmentId = employee.DepartmentId;
                                userLogin.PositionId = employee.PositionId;
                                userLogin.ProjectId = employee.ProjectId;
                                userLogin.FullName = employee.FullName;
                                userLogin.Email = employee.Email;
                                userLogin.Phone = employee.Phone;
                                userLogin.Address = employee.Address;
                                userLogin.Avata = (employee.Avata != null && employee.Avata != "") ? employee.Avata : AppEnum.DEFAULT_AVATAR; ;
                                userLogin.Birthday = employee.Birthday;
                                userLogin.CardId = employee.CardId;
                                //var project = await db.Project.Where(e => e.ProjectId == userLogin.ProjectId).FirstOrDefaultAsync();
                                var project = await _projectRepo.FindByProjectIdAsync(userLogin.ProjectId, cancellationToken);
                                userLogin.ProjectName = project != null ? project.Name : "";
                                //var department = await db.Department.Where(e => e.DepartmentId == userLogin.DepartmentId).FirstOrDefaultAsync();
                                var department = await _departmentRepo.FindByDepartmentIdAsync(userLogin.DepartmentId, cancellationToken);
                                userLogin.DepartmentName = department != null ? department.Name : "";
                                //var position = await db.TypeAttributeItem.Where(e => e.TypeAttributeItemId == userLogin.PositionId).FirstOrDefaultAsync();
                                var position = await _typeAttributeItem.FindByTypeAttributeItemIdAsync(userLogin.PositionId, cancellationToken);
                                userLogin.PositionName = position != null ? position.Name : "";
                            }

                            var userId = userLogin.UserId;
                            List<MenuDTO> listFunctionRole = new List<MenuDTO>();
                            //lấy danh sách quyền theo chức năng, nếu danh sách quyền theo chức năng null thì lấy
                            //danh sách quyền theo nhóm quyền

                            if (userLogin.IsRoleGroup != true)
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
                                        userLogin.RoleCode = role.Code;
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
                            //userLogin.listMenus = UserService.CreateMenu(listFunctionRole, 0);
                        }
                        else
                        {
                            userLogin = null;
                        }
                    }

                    if (userLogin != null)
                    {
                        userLogin.baseApi = _configuration["AppSettings:baseApi"];
                        userLogin.baseUrlImgPicture = _configuration["AppSettings:baseUrlImgPicture"];
                        userLogin.baseUrlImgThumbProduct = _configuration["AppSettings:baseUrlImgThumbProduct"];
                        userLogin.baseUrlImgThumbNews = _configuration["AppSettings:baseUrlImgThumbNews"];
                        //check if user active phon
                        if (userLogin.RegisterCode != null && userLogin.IsPhoneConfirm != true)
                        {
                            //def.meta = new Meta(218, "Account Not Active!");
                            //def.data = user;
                            //return def;
                            throw new CommonException("Tài khoản chưa được kích hoạt!", 218, ApiConstants.ErrorCode.ERROR_USER_NOT_ACTIVE);
                        }

                        //check if user lock
                        if (userLogin.Status == (int)AppEnum.EntityStatus.LOCK)
                        {
                            //def.meta = new Meta(223, "Account locked!");
                            //return def;
                            throw new CommonException("Tài khoản đã bị khóa!", 223, ApiConstants.ErrorCode.ERROR_USER_LOCKED);
                        }

                        //lấy số thông báo và số email chưa đọc

                        //var badgeNotification = db.Action.Where(a => a.UserId == userLogin.UserId && a.Status == (int)Const.Status.NORMAL
                        //    && a.Type == (int)Const.TypeAction.NOTIFICATION).ToList();
                        //var badgeMail = db.Action.Where(a => a.UserId == userLogin.CustomerId && a.Status == (int)Const.Status.NORMAL
                        //    && a.Type == (int)Const.TypeAction.MAIL).ToList();
                        try
                        {
                            HttpClient client = _factory.CreateClient();
                            var requestNoti = new HttpRequestMessage(HttpMethod.Get, $"{userLogin.baseApi}api/cms/action/user/{userLogin.UserId}");
                            var response = await client.SendAsync(requestNoti);
                            if (response.IsSuccessStatusCode)
                            {
                                var contents = response.Content.ReadAsStringAsync().Result;
                                var json = JsonConvert.DeserializeObject<dynamic>(contents);

                                if (json["data"] != null)
                                {
                                    userLogin.badgeNotification = json["metadata"];
                                }
                            }
                            //    RestClient client = new RestClient(userLogin.baseApi);
                            //_logger.LogInformation("Call api: api/cms/action/user/" + userLogin.UserId);
                            //var requestA = new RestRequest("api/cms/action/user/" + userLogin.UserId, Method.GET);
                            ////requestA.AddJsonBody(requestCSKH);
                            //requestA.AddHeader("Content-Type", "application/json");
                            ////requestA.AddHeader("token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c24iOiJ2bWd0ZXN0MSIsInNpZCI6ImZkMTA5NWM3LTc0NGQtNGUzZS1iODRlLTg2MWUzZDAzMmM2ZiIsIm9idCI6IiIsIm9iaiI6IiIsIm5iZiI6MTU4MzgzNDA3OCwiZXhwIjoxNTgzODM3Njc4LCJpYXQiOjE1ODM4MzQwNzh9.5wS0_iqzHypsZaGFTBtVyTCyHegSWj1onY-hQqw7b40");
                            //IRestResponse response = await client.ExecuteAsync(requestA);
                            //var content = response.Content; // raw content as string
                            //if (content != null)
                            //{
                            //    JObject json = JObject.Parse(content);

                            //    userLogin.badgeNotification = json.Count;
                            //    //userLogin.badgeMail = badgeMail.Count;
                            //}
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("Fail call api get count notification:" + e.Message);
                        };

                        userLogin.LanguageId = request.TypeLanguage;

                        var claims = new List<Claim>
                                {
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, userLogin.UserId.ToString()),
                                    new Claim(ClaimTypes.Name, userLogin.FullName != null ? userLogin.FullName : ""),
                                    new Claim("UserId", userLogin.UserId.ToString()),
                                    new Claim("UserMapId", userLogin.UserMapId != null ? userLogin.UserMapId.ToString() : ""),
                                    new Claim("Type", userLogin.Type != null ? userLogin.Type.ToString() : ""),
                                    new Claim("Username", userLogin.UserName != null ? userLogin.UserName.ToString() : ""),
                                    new Claim("Language", userLogin.LanguageId != null ? userLogin.LanguageId.ToString() : ""),
                                    new Claim("AccessKey", userLogin.access_key != null ? userLogin.access_key : ""),
                                    new Claim("RoleCode", userLogin.RoleCode != null ? userLogin.RoleCode.ToString() : ""),
                                    new Claim("ProjectId", userLogin.ProjectId != null ? userLogin.ProjectId.ToString() : ""),
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

                        userLogin.access_token = new JwtSecurityTokenHandler().WriteToken(token);


                        //var de = user.FirstOrDefault();
                        try
                        {
                            //Update lại ngôn ngữ và số lần login
                            user.LanguageId = request.TypeLanguage;
                            user.CountLogin = (user.CountLogin != null ? user.CountLogin : 0) + 1;

                            //db.Update(user);
                            //await db.SaveChangesAsync();
                            _userRepo.Update(user);
                            await _unitOfWork.CommitChangesAsync();

                            //Ghi log login
                            //Lấy danh sách người nhận là tất cả admin
                            //var userRoles = db.UserRole.Where(e => e.RoleId == 1 && e.Status != (int)Const.Status.DELETED).ToList();
                            var userRoles = await _userRoleRepo.GetListDataByCondition("RoleId=1");
                            if (userRoles.Count > 0)
                            {
                                var reqMessages = new List<DtoNotificationActionCreateQueue>();
                                foreach (var item in userRoles)
                                {
                                    //create action
                                    //Models.EF.Action action = new Models.EF.Action();
                                    //action.ActionId = Guid.NewGuid();
                                    //action.ActionName = "đăng nhập vào app";
                                    //action.ActionType = "LOGIN";
                                    //action.TargetId = userLogin.UserId + "";
                                    //action.TargetType = "USER";
                                    //action.Logs = JsonConvert.SerializeObject(userLogin);
                                    //action.Time = 0;
                                    //action.Type = (int)Const.TypeAction.ACTION;
                                    //action.CreatedAt = DateTime.Now;
                                    //action.UserPushId = userLogin.UserId;
                                    //action.UserId = item.UserId;
                                    //action.Status = (int)Const.Status.NORMAL;
                                    //await db.Action.AddAsync(action);
                                    //await db.SaveChangesAsync();

                                    reqMessages.Add(new DtoNotificationActionCreateQueue(
                                    $"đăng nhập vào app",
                                    "LOGIN",
                                    item.Id.ToString(),
                                    "USER",
                                    null,
                                    0,
                                    JsonConvert.SerializeObject(item),
                                    "192.168.1.1",
                                    0,
                                    TypeAction.ACTION,
                                    item.UserId));

                                    ////push action
                                    //Models.Data.Firebase.pushAction(action);


                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("Fail login" + e.Message);
                        };

                        //update firestore
                        //try
                        //{
                        //    await IOITResident.Models.Data.Firebase.UpdateInfoUser(user.UserId);
                        //}
                        //catch { }

                        //def.data = userLogin;
                        //def.meta = new Meta(200, "success");
                        //return def;
                        return _mapper.Map<ResUserLoginApp>(userLogin);
                    }
                    else
                    {
                        userLogin = new ResUserLoginApp();
                        userLogin.MetaCode = 200;
                        userLogin.MetaMess = ApiConstants.MessageResource.ACCTION_SUCCESS;
                        //check if username exist
                        //var existed = db.User.Where(e => e.UserName == Username && e.Status != (int)Const.Status.DELETED).FirstOrDefault();
                        var existed = _userRepo.All().Where(e => e.UserName == Username && e.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                        //var existed = new User();
                        if (request.TypeUser == (int)AppEnum.TypeUser.RESIDENT_MAIN || request.TypeUser == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                        {
                            string userName2 = Utils.ConvertPhone(Username);
                            existed = _userRepo.All().Where(e => (e.UserName.Trim() == Username || e.UserName.Trim() == userName2)
                            && (e.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || e.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                            && e.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                            //existed = await _userRepo.CheckUserLoginAppAsync(request.Username, password, 1);
                        }
                        else if (request.TypeUser == (int)AppEnum.TypeUser.MANAGEMENT || request.TypeUser == (int)AppEnum.TypeUser.TECHNICIANS)
                        {
                            existed = _userRepo.All().Where(e => e.UserName == Username
                            && (e.Type == (int)AppEnum.TypeUser.MANAGEMENT || e.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                            && e.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                            //existed = await _userRepo.CheckUserLoginAppAsync(request.Username, password, 2);
                        }
                        if (existed != null)
                        {
                            userLogin.UserId = existed.Id;
                            userLogin.UserMapId = existed.UserMapId;
                            userLogin.DepartmentId = existed.DepartmentId;
                            userLogin.PositionId = existed.PositionId;
                            userLogin.UserName = existed.UserName;
                            userLogin.LanguageId = existed.LanguageId;
                            userLogin.Type = existed.Type;
                            userLogin.CountLogin = existed.CountLogin;
                            userLogin.Status = (byte)existed.Status;
                            userLogin.IsPhoneConfirm = existed.IsPhoneConfirm;
                            userLogin.RegisterCode = existed.RegisterCode;

                            if (existed.RegisterCode != null && existed.IsPhoneConfirm != true)
                            {
                                //def.meta = new Meta(218, "Account Not Active!");
                                //def.data = existed;
                                //return def;
                                throw new CommonException("Tài khoản chưa được kích hoạt!", 218, ApiConstants.ErrorCode.ERROR_USER_NOT_ACTIVE);
                            }
                            if (existed.Password == null)
                            {
                                //def.meta = new Meta(216, "Not Update Password new!");
                                //def.data = existed;
                                //return def;
                                throw new CommonException("Vui lòng nhập mật khẩu mới!", 216, ApiConstants.ErrorCode.ERROR_PASSWORD_NEW_EMPTY);
                            }
                            else
                            {
                                //def.meta = new Meta(213, "Invalid data!");
                                //return def;
                                throw new CommonException("Sai dữ liệu. Vui lòng thử lại sau!", 213, ApiConstants.ErrorCode.ERROR_USER_LOGIN_FAILED);
                            }
                        }
                        else
                        {
                            //def.meta = new Meta(404, "Not found!");
                            //return def;
                            throw new CommonException(Resources.USER_NOT_FOUND, 404, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                        }
                    }
                }
                else
                {
                    //def.meta = new Meta(404, "Not Found!");
                    //return def;
                    throw new CommonException(Resources.USER_NOT_FOUND, 404, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }
                //}

                //return _mapper.Map<ResUserLoginApp>(userLogin);
            }

        }
    }
}
