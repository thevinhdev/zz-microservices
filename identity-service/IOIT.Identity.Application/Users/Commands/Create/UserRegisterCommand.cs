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
using IOIT.Shared.Helpers;
using IOIT.Shared.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class UserRegisterCommand : IRequest<DefaultResponse>
    {
        public string PhoneMain { get; set; }
        public byte? LanguageId { get; set; }

        public class Validation : AbstractValidator<UserRegisterCommand>
        {
            public Validation()
            {
                RuleFor(x => x.PhoneMain).NotEmpty().WithMessage("PhoneMain not empty");
                //RuleFor(x => x.LanguageId).NotEmpty().WithMessage("LanguageId not empty");
            }
        }

        public class UserRegisterHandler : IRequestHandler<UserRegisterCommand, DefaultResponse>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IConfiguration _configuration;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IResidentAsyncRepository _residentRepo;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly IOptionsSnapshot<AppsetingOption> _options;
            private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

            public UserRegisterHandler(IMapper mapper,
                IUserAsyncRepository userRepo,
                IUnitOfWork unitOfWork,
                IConfiguration configuration,
                IResidentAsyncRepository residentRepo,
                IApartmentMapAsyncRepository amRepo,
                IOptionsSnapshot<AppsetingOption> options
                )
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _configuration = configuration;
                _userRepo = userRepo;
                _residentRepo = residentRepo;
                _amRepo = amRepo;
                _options = options;
            }

            public async Task<DefaultResponse> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
            {
                //string Username = request.Username;
                //ResUserRegisterApp user = new ResUserRegisterApp();
                //using (var db = new IOITResidentGateContext())
                //{
                DefaultResponse def = new DefaultResponse();
                def.Meta = new Meta(200, ApiConstants.MessageResource.ACCTION_SUCCESS);

                string phoneMain2 = Utils.ConvertPhone(request.PhoneMain);
                //check xem sdt đó có phù hợp với chủ hộ nào ko
                var checkResident = await (from res in _residentRepo.All()
                                           join ar in _amRepo.All() on res.Id equals ar.ResidentId
                                           //where Utils.CheckPhone(request.PhoneMain, res.Phone)
                                           where (res.Phone == request.PhoneMain || res.Phone == phoneMain2)
                                          && (ar.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN || ar.Type == (int)AppEnum.TypeResident.RESIDENT_MEMBER)
                                           && res.Status == AppEnum.EntityStatus.NORMAL
                                           && ar.Status != AppEnum.EntityStatus.DELETED
                                           select new
                                           {
                                               res.Id,
                                               res.FullName,
                                               res.Avata,
                                               ar.ProjectId,
                                               res.CardId,
                                               res.Email,
                                               res.Address,
                                           }).FirstOrDefaultAsync();

                //var checkResident = await _residentRepo.CheckPhoneResidentMainAsync(request.PhoneMain, cancellationToken);
                if (checkResident == null)
                {
                    //def.meta = new Meta(215, "Resident main not found!");
                    //return Ok(def);
                    throw new CommonException("Resident main not found!", 215, ApiConstants.ErrorCode.ERROR_RESIDENTMAIN_NOT_EXIST);
                }

                //check xem tài khoản đã tồn tại chưa or đã active chưa or đã đổi mk mới chưa
                //var checkExistUn = db.User.Where(e => (e.Type == (int)Const.TypeUser.RESIDENT_MAIN || e.Type == (int)Const.TypeUser.RESIDENT_GUEST)
                //&& Utils.CheckPhone(data.PhoneMain, e.UserName) && e.Status != (int)Const.Status.DELETED).FirstOrDefault();
                var checkExistUn = await _userRepo.FindByPhoneUsernameAsync(request.PhoneMain, 1, cancellationToken);
                if (checkExistUn != null && checkExistUn.IsDeletedByGuest == false)
                {
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
                }
                //nếu ok thì gửi gửi mã OTP + tạo tài khoản
                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                User user = new User();
                string code = Utils.RandomNumber(6);
                if (checkExistUn.IsDeletedByGuest == true)
                {
                    checkExistUn.UserName = phoneMain2;
                    if (checkExistUn.TypeThird == (int)AppEnum.TypeRegister.PHONE_NUMBER)
                    {
                        checkExistUn.Phone = checkExistUn.UserName;
                        checkExistUn.IsPhoneConfirm = false;
                    }
                    else
                        checkExistUn.IsPhoneConfirm = true;

                    checkExistUn.KeyRandom = Utils.RandomString(8);
                    checkExistUn.RegEmail = Utils.RandomString(8);
                    checkExistUn.RegisterCode = code;
                    checkExistUn.IsEmailConfirm = true;
                    checkExistUn.Type = (int)AppEnum.TypeUser.RESIDENT_MAIN;
                    checkExistUn.DepartmentId = 1;
                    checkExistUn.CountLogin = 0;
                    checkExistUn.LanguageId = request.LanguageId;
                    checkExistUn.LastLoginAt = DateTime.Now;
                    checkExistUn.UpdatedAt = DateTime.Now;
                    checkExistUn.IsDeletedByGuest = false;
                    _userRepo.Update(checkExistUn);
                    await _unitOfWork.CommitChangesAsync();
                    user = checkExistUn;
                }
                else
                {
                    //var entity = _mapper.Map<User>(request);
                    user.UserMapId = checkResident.Id;
                    user.FullName = checkResident.FullName;
                    user.Avata = checkResident.Avata;
                    //user.PositionId = checkResident.RelationshipId;
                    user.ProjectId = checkResident.ProjectId;
                    user.CardId = checkResident.CardId;
                    user.Email = checkResident.Email;
                    user.Address = checkResident.Address;
                    user.RoleMax = 9999;
                    user.RoleLevel = 99;
                    user.IsRoleGroup = false;
                    user.CreatedById = 1;
                    user.UpdatedById = 1;
                    user.TypeThird = (int)AppEnum.TypeRegister.PHONE_NUMBER;
                    user.UserName = phoneMain2;
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
                    user.Type = (int)AppEnum.TypeUser.RESIDENT_MAIN;
                    user.DepartmentId = 1;
                    user.CountLogin = 0;
                    user.LanguageId = request.LanguageId;
                    user.LastLoginAt = DateTime.Now;
                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    user.Status = AppEnum.EntityStatus.NORMAL;

                    await _userRepo.AddAsync(user);
                    await _unitOfWork.CommitChangesAsync();
                }
                //await db.User.AddAsync(user);
                try
                {
                    //await db.SaveChangesAsync();
                    if (user.Id > 0)
                    {
                        //push mail
                        if (user.TypeThird == (int)AppEnum.TypeRegister.PHONE_NUMBER)
                        {
                            string baseApi = _options.Value.baseApi;

                            //bool check = sendEmail(user.Email, "Quý khách", code, "TNS - Xác thực tài khoản");
                            //gửi OTP
                            string mes = "TNSPLUS: " + user.RegisterCode + " la ma OTP kich hoat Dang ky tai khoan cua ban";

                            try
                            {
                                ResSendOTP resSendOTP = new ResSendOTP();
                                resSendOTP.phone = request.PhoneMain;
                                resSendOTP.mess = mes;
                                resSendOTP.code = user.RegisterCode;
                                resSendOTP.projectId = user.ProjectId;
                                RestClient client = new RestClient(baseApi);
                                var requestA = new RestRequest("api/app/SMSBrandname/sendOTP2", Method.POST);
                                requestA.AddJsonBody(resSendOTP);
                                requestA.AddHeader("Content-Type", "application/json");

                                var logDatapayload = new LogEventInfo(NLog.LogLevel.Info, "", $"Start call api with url: {baseApi}api/app/SMSBrandname/sendOTP2, payload: {JsonConvert.SerializeObject(resSendOTP)}, at: {DateTime.Now.ToString("yyyyMMddHHmmss")}");
                                logDatapayload.Properties.Add("ErrorCode", ApiConstants.ErrorCode.LOG_DATA_OPT);

                                _logger.Info(logDatapayload);

                                IRestResponse response = await client.ExecuteAsync(requestA);

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

                                        def.Meta = new Meta(200, ApiConstants.MessageResource.ACCTION_SUCCESS);
                                        def.Data = _mapper.Map<ResUserRegisterApp>(user);
                                        //return Ok(def);
                                        return def;
                                    }
                                    else
                                    {
                                        throw new CommonException($"Lấy mã OPT thất bại. Response: {json["meta"]["error_message"]}", 230, ApiConstants.ErrorCode.ERROR_OPT_GET_FAILED);
                                    }
                                }
                                else
                                {
                                    throw new CommonException($"Lấy mã OPT thất bại.", 230, ApiConstants.ErrorCode.ERROR_OPT_GET_FAILED);
                                }
                            }
                            catch (Exception e)
                            {
                                throw new CommonException($"Lấy mã OPT thất bại. ERROR: {e.Message}", 230, ApiConstants.ErrorCode.ERROR_OPT_GET_FAILED);
                            };

                            //ResponseCSKH sendSMS = await SendSMS.SendCSKH(data.PhoneMain, mes, user.RegisterCode);
                            //if (sendSMS.errorCode == "000")
                            //{
                            //user.UserName = sendSMS.sendMessage.to;
                            //user.Phone = sendSMS.sendMessage.to;
                            //db.User.Update(user);
                            //await db.SaveChangesAsync();
                            //var entity = _mapper.Map<User>(user);
                            //    _userRepo.Update(entity);
                            //    await _unitOfWork.CommitChangesAsync();
                            //    _unitOfWork.CommitTransaction();

                            //    return _mapper.Map<ResUserRegisterApp>(user);
                            //def.meta = new Meta(200, "Success");
                            //def.data = user;
                            //return Ok(def);
                            //}
                            //else
                            //{
                            //    //def.data = data;
                            //    //def.meta = new Meta(230, "Not send sms");
                            //    //return Ok(def);
                            //    throw new CommonException(230, "Not send sms");
                            //}
                        }
                        else
                        {
                            //transaction.Commit();
                            _unitOfWork.CommitTransaction();
                        }

                        //def.meta = new Meta(200, "Success");
                        //def.data = data;
                        def.Data = _mapper.Map<ResUserRegisterApp>(user);
                        return def; ;
                    }
                    else
                    {
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException("Bad Request", ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_REGISTRATION_USER_FAILED);
                    }
                }
                catch (DbUpdateException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_REGISTRATION_USER_FAILED);
                }

            }
        }

    }
}
