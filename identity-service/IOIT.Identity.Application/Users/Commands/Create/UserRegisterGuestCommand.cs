using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces.Options;
using IOIT.Identity.Application.Common.Service;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels;
using IOIT.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Commands.Create
{
    public class UserRegisterGuestCommand : IRequest<DefaultResponse>
    {
        public int ApartmentId { get; set; }
        public int FloorId { get; set; }
        public int TowerId { get; set; }
        public int ProjectId { get; set; }
        public string PhoneMain { get; set; }
        public string PhoneGuest { get; set; }
        public byte? LanguageId { get; set; }
        public string FullName { get; set; }
        public string CardId { get; set; }
        public long? ResidentId { get; set; }

        public class Validation : AbstractValidator<UserRegisterGuestCommand>
        {
            public Validation()
            {
                RuleFor(x => x.ProjectId).NotEmpty().WithMessage("ProjectId not empty");
                RuleFor(x => x.PhoneMain).NotEmpty().WithMessage("PhoneMain not empty");
            }
        }

        public class UserRegisterHandler : IRequestHandler<UserRegisterGuestCommand, DefaultResponse>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
            private readonly IResidentAsyncRepository _residentRepo;
            //private readonly IApartmentMapAsyncRepository _amRepo;
            //private readonly IProjectAsyncRepository _projectRepo;
            //private readonly IDepartmentAsyncRepository _departmentRepo;
            IOptionsSnapshot<AppsetingOption> _options;

            public UserRegisterHandler(IMapper mapper, IUserAsyncRepository userRepo,
                IUnitOfWork unitOfWork,
                //IAsyncRepository<Employee> empRepo,
                //IUserRoleAsyncRepository userRoleRepo, IFunctionRoleAsyncRepository funcRoleRepo,
                //IFunctionAsyncRepository funcRepo, IAsyncRepository<Role> roleRepo,
                IResidentAsyncRepository residentRepo,
                //IApartmentMapAsyncRepository amRepo,
                //IProjectAsyncRepository projectRepo, IDepartmentAsyncRepository departmentRepo
                IOptionsSnapshot<AppsetingOption> options
                )
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                //_empRepo = empRepo;
                //_userRoleRepo = userRoleRepo;
                //_funcRoleRepo = funcRoleRepo;
                //_funcRepo = funcRepo;
                //_roleRepo = roleRepo;
                _residentRepo = residentRepo;
                //_amRepo = amRepo;
                //_projectRepo = projectRepo;
                //_departmentRepo = departmentRepo;
                _options = options;
            }

            public async Task<DefaultResponse> Handle(UserRegisterGuestCommand request, CancellationToken cancellationToken)
            {
                //using (var db = new IOITResidentGateContext())
                //{
                //request.PhoneMain = Utils.ConvertPhone(request.PhoneMain);
                //request.PhoneGuest = Utils.ConvertPhone(request.PhoneGuest);
                //check xem sdt đó có phù hợp với số của căn hộ nào ko
                //var checkResident = await (from res in db.Resident
                //                           join ar in db.ApartmentMap on res.ResidentId equals ar.ResidentId
                //                           where ar.ProjectId == data.ProjectId && ar.TowerId == data.TowerId
                //                           && ar.FloorId == data.FloorId && ar.ApartmentId == data.ApartmentId
                //                           && ar.Type == (int)Const.TypeResident.RESIDENT_MAIN
                //                           && res.Status == (int)Const.Status.NORMAL
                //                           && ar.Status != (int)Const.Status.DELETED
                //                           select res).FirstOrDefaultAsync();
                DefaultResponse def = new DefaultResponse();
                def.Meta = new Meta(200, ApiConstants.MessageResource.ACCTION_SUCCESS);

                var checkResident = await _residentRepo.CheckResidentExitAsync(request.ProjectId, request.TowerId,
                    request.FloorId, request.ApartmentId, cancellationToken);

                if (checkResident == null)
                {
                    //def.meta = new Meta(215, "Resident main not found!");
                    //return Ok(def);
                    throw new CommonException("Không tồn tại chủ hộ!", 215, ApiConstants.ErrorCode.ERROR_RESIDENTMAIN_NOT_EXIST);
                }

                if (!Utils.CheckPhone(request.PhoneMain, checkResident.Phone))
                {
                    //def.meta = new Meta(217, "Phone main not found!");
                    //return Ok(def);
                    throw new CommonException("Không tìm thấy số điện thoại chủ hộ!", 217, ApiConstants.ErrorCode.ERROR_RESIDENT_PHONE_NOT_FOUND);
                }

                //check xem tài khoản đã tồn tại chưa or đã active chưa or đã đổi mk mới chưa
                //var checkExistUn = db.User.Where(e => Utils.CheckPhone(data.PhoneGuest, e.UserName)
                //&& (e.Type == (int)Const.TypeUser.RESIDENT_MAIN || e.Type == (int)Const.TypeUser.RESIDENT_GUEST)
                //&& e.Status != (int)Const.Status.DELETED).FirstOrDefault();
                var checkExistUn = await _userRepo.FindByPhoneUsernameAsync(request.PhoneGuest, 1, cancellationToken);
                if (checkExistUn != null)
                {
                    //check xem tài khoản đó đã active chủ hộ chưa
                    if (checkExistUn.Status == AppEnum.EntityStatus.LOCK)
                        //def.meta = new Meta(223, "Not Active Resident main!");
                        throw new CommonException("Chủ hộ chưa được kích hoạt!", 223, ApiConstants.ErrorCode.ERROR_RESIDENTMAIN_NOT_ACTIVE);
                    //nếu đã tồn tại, check đã active chưa
                    if (checkExistUn.IsPhoneConfirm == true)
                    {
                        //nếu đã active check đã tạo pass mới chưa
                        if (checkExistUn.Password == null)
                            //def.meta = new Meta(216, "Not Update Password new!");
                            throw new CommonException("Chưa cập nhật mật khẩu mới!", 216, ApiConstants.ErrorCode.ERROR_PASSWORD_NEW_EMPTY);
                        else
                            //def.meta = new Meta(212, "Username Exist!");
                            throw new CommonException("Tài khoản đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_USERNAME_EXISTED);
                    }
                    else
                    {
                        if (checkExistUn.RegisterCode == null)
                            //def.meta = new Meta(212, "Username Exist!");
                            throw new CommonException("Tài khoản đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_USERNAME_EXISTED);
                        else
                            //def.meta = new Meta(218, "Account Not Active!");
                            throw new CommonException("Tài khoản chưa được kích hoạt!", 218, ApiConstants.ErrorCode.ERROR_USER_NOT_ACTIVE);
                    }
                    def.Data = _mapper.Map<ResUserRegisterApp>(checkExistUn);
                    return def;
                }
                //nếu ok thì gửi gửi mã OTP + tạo tài khoản
                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                //check xem khách thuê đã tồn tại chưa
                //var checkResidentGuest = await (from res in db.Resident
                //                                join ar in db.ApartmentMap on res.ResidentId equals ar.ResidentId
                //                                where Utils.CheckPhone(data.PhoneGuest, res.Phone)
                //                                    && ar.ApartmentId == data.ApartmentId
                //                                    && ar.ProjectId == data.ProjectId
                //                                    && ar.TowerId == data.TowerId
                //                                    && ar.FloorId == data.FloorId
                //                                    && (ar.Type == (int)Const.TypeResident.RESIDENT_MEMBER
                //                                    || ar.Type == (int)Const.TypeResident.RESIDENT_GUEST_MEMBER)
                //                                    && res.Status == (int)Const.Status.NORMAL
                //                                    && ar.Status != (int)Const.Status.DELETED
                //                                select res).FirstOrDefaultAsync();
                var checkResidentGuest = await _residentRepo.CheckPhoneResidentGuestAsync(request.PhoneGuest, request.ProjectId, request.TowerId,
                request.FloorId, request.ApartmentId, cancellationToken);

                long? residentId = null;
                //if (checkResidentGuest == null)
                //{
                //    //Tạo khách thuê
                //    Resident resident = new Resident();
                //    resident.FullName = request.FullName;
                //    resident.Phone = request.PhoneGuest;
                //    resident.CardId = request.CardId;
                //    resident.Type = (int)AppEnum.TypeResident.RESIDENT_MEMBER;
                //    resident.CreatedAt = DateTime.Now;
                //    resident.UpdatedAt = DateTime.Now;
                //    resident.CreatedById = 1;
                //    resident.UpdatedById = 1;
                //    resident.Status = AppEnum.EntityStatus.NORMAL;
                //    await db.Resident.AddAsync(resident);
                //    await db.SaveChangesAsync();
                //    residentId = resident.Id;

                //    //Tạo map phòng
                //    ApartmentMap apartmentMap = new ApartmentMap();
                //    apartmentMap.Id = Guid.NewGuid();
                //    apartmentMap.ApartmentId = request.ApartmentId;
                //    apartmentMap.FloorId = request.FloorId;
                //    apartmentMap.TowerId = request.TowerId;
                //    apartmentMap.ProjectId = request.ProjectId;
                //    apartmentMap.ResidentId = residentId;
                //    apartmentMap.Type = (int)AppEnum.TypeResident.RESIDENT_MEMBER;
                //    apartmentMap.CreatedAt = DateTime.Now;
                //    apartmentMap.CreatedById = 1;
                //    apartmentMap.UpdatedById = 1;
                //    apartmentMap.Status = AppEnum.EntityStatus.TEMP;
                //    await db.ApartmentMap.AddAsync(apartmentMap);
                //    await db.SaveChangesAsync();
                //}
                //else
                residentId = checkResidentGuest.Id;

                string code = Utils.RandomNumber(6);
                User user = new User();
                user.UserMapId = residentId;
                user.FullName = checkResidentGuest.FullName;
                user.DepartmentId = request.ApartmentId;
                user.RoleMax = 9999;
                user.RoleLevel = 99;
                user.IsRoleGroup = false;
                user.CreatedById = 1;
                user.UpdatedById = 1;
                user.TypeThird = (int)AppEnum.TypeRegister.PHONE_NUMBER;
                user.UserName = request.PhoneGuest;
                if (user.TypeThird == (int)AppEnum.TypeRegister.PHONE_NUMBER)
                {
                    user.Phone = user.UserName;
                    user.IsPhoneConfirm = false;
                }
                else
                    user.IsPhoneConfirm = true;
                user.KeyRandom = Utils.RandomString(8);
                user.RegEmail = Utils.RandomString(8);
                user.RegisterCode = code;
                user.IsEmailConfirm = true;
                user.Type = (int)AppEnum.TypeUser.RESIDENT_GUEST;
                user.DepartmentId = 1;
                user.CountLogin = 0;
                user.LanguageId = request.LanguageId;
                user.LastLoginAt = DateTime.Now;
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                user.Status = AppEnum.EntityStatus.LOCK;
                //await db.User.AddAsync(user);
                await _userRepo.AddAsync(user);
                await _unitOfWork.CommitChangesAsync();
                try
                {
                    //await db.SaveChangesAsync();
                    if (user.Id > 0)
                    {
                        //push mail
                        if (user.TypeThird == (int)AppEnum.TypeRegister.PHONE_NUMBER)
                        {
                            //gửi OTP
                            string mes = "TNSPLUS: " + user.RegisterCode + " la ma OTP kich hoat Dang ky tai khoan cua ban";

                            try
                            {
                                ResSendOTP resSendOTP = new ResSendOTP();
                                resSendOTP.phone = user.Phone;
                                resSendOTP.mess = mes;
                                resSendOTP.code = user.RegisterCode;
                                string baseApi = _options.Value.baseApi;
                                RestClient client = new RestClient(baseApi);
                                var requestA = new RestRequest("api/app/SMSBrandname/sendOTP2", Method.Post);
                                requestA.AddJsonBody(resSendOTP);
                                requestA.AddHeader("Content-Type", "application/json");
                                //requestA.AddHeader("token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c24iOiJ2bWd0ZXN0MSIsInNpZCI6ImZkMTA5NWM3LTc0NGQtNGUzZS1iODRlLTg2MWUzZDAzMmM2ZiIsIm9idCI6IiIsIm9iaiI6IiIsIm5iZiI6MTU4MzgzNDA3OCwiZXhwIjoxNTgzODM3Njc4LCJpYXQiOjE1ODM4MzQwNzh9.5wS0_iqzHypsZaGFTBtVyTCyHegSWj1onY-hQqw7b40");

                                var logDatapayload = new LogEventInfo(NLog.LogLevel.Info, "", $"Start call api with url: {baseApi}api/app/SMSBrandname/sendOTP2, payload: {JsonConvert.SerializeObject(resSendOTP)}, at: {DateTime.Now.ToString("yyyyMMddHHmmss")}");
                                logDatapayload.Properties.Add("ErrorCode", ApiConstants.ErrorCode.LOG_DATA_OPT);

                                _logger.Info(logDatapayload);

                                var response = await client.ExecuteAsync(requestA);

                                var content = response.Content; // raw content as string

                                var logDataResponse = new LogEventInfo(NLog.LogLevel.Info, "", $"Successfully call api: {baseApi}api/app/SMSBrandname/sendOTP2, payload: {JsonConvert.SerializeObject(resSendOTP)}, response: {content}");
                                logDataResponse.Properties.Add("ErrorCode", ApiConstants.ErrorCode.LOG_DATA_OPT);

                                _logger.Info(logDataResponse);

                                if (content != null)
                                {

                                    JObject json = JObject.Parse(content);

                                    if (json["meta"]["error_code"].ToString() == "200")
                                    {
                                        user.UserName = json["data"]["sendMessage"]["to"].ToString();
                                        user.Phone = json["data"]["sendMessage"]["to"].ToString();
                                        //db.User.Update(user);
                                        //await db.SaveChangesAsync();
                                        //var entity = _mapper.Map<User>(user);
                                        _userRepo.Update(user);
                                        await _unitOfWork.CommitChangesAsync();
                                        _unitOfWork.CommitTransaction();

                                        def.Data = _mapper.Map<ResUserRegisterApp>(user);
                                        return def;
                                    }
                                    else
                                    {
                                        throw new CommonException($"Lấy mã OPT thất bại. Response: {json["meta"]["error_message"]}", 230, ApiConstants.ErrorCode.ERROR_OPT_GET_FAILED);
                                    }
                                }
                                else
                                {
                                    throw new CommonException($"Lấy mã OPT không thành công! Vui lòng thử lại.", 230, ApiConstants.ErrorCode.ERROR_OPT_GET_FAILED);
                                }
                            }
                            catch (Exception e)
                            {
                                throw new CommonException($"Lấy mã OPT thất bại. ERROR: {e.Message}", 230, ApiConstants.ErrorCode.ERROR_OPT_GET_FAILED);
                            };
                            //bool check = sendEmail(user.Email, "Quý khách", code, "TNS - Xác thực tài khoản");
                            //gửi OTP
                            //string mes = "TNSPLUS: " + user.RegisterCode + " la ma OTP kich hoat Dang ky tai khoan cua ban";
                            //ResponseCSKH sendSMS = await SendSMS.SendCSKH(data.PhoneGuest, mes, user.RegisterCode);
                            //if (sendSMS.errorCode == "000")
                            //{
                            //    user.UserName = sendSMS.sendMessage.to;
                            //    user.Phone = sendSMS.sendMessage.to;
                            //    db.User.Update(user);
                            //    await db.SaveChangesAsync();
                            //    transaction.Commit();

                            //def.meta = new Meta(200, "Success");
                            //def.data = user;
                            //return Ok(def);
                            //var entity = _mapper.Map<User>(user);
                            //_userRepo.Update(entity);
                            //await _unitOfWork.CommitChangesAsync();
                            //_unitOfWork.CommitTransaction();

                            //return _mapper.Map<ResUserRegisterApp>(user);
                            //}
                            //else
                            //{
                            //    def.data = data;
                            //    def.meta = new Meta(230, "Not send sms");
                            //    return Ok(def);
                            //}
                        }
                        else
                        {
                            //transaction.Commit();
                            _unitOfWork.RollbackTransaction();
                        }

                        def.Data = _mapper.Map<ResUserRegisterApp>(user);
                        return def;
                    }
                    else
                    {
                        //transaction.Rollback();
                        //def.meta = new Meta(400, "Bad Request");
                        //return Ok(def);
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException("Đăng kí tài khoản thất bại", ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_REGISTRATION_USER_FAILED);
                    }

                }
                catch (DbUpdateException e)
                {
                    //    log.Error("DbUpdateException:" + e);
                    //    transaction.Rollback();
                    //    def.meta = new Meta(500, "Internal Server Error");
                    //    return Ok(def);
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_REGISTRATION_USER_FAILED);
                }
                //}
                //}
                //return _mapper.Map<ResUserRegisterApp>(user);
            }
        }
    }
}
