using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces.Options;
using IOIT.Identity.Application.Models;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Commands.Update
{
    public class SendChangePassCodeCommand : IRequest<UserDT>
    {
        public string phone { get; set; }

        public class Validation : AbstractValidator<SendChangePassCodeCommand>
        {
            public Validation()
            {
                RuleFor(x => x.phone).NotEmpty().WithMessage("Phone not empty");
            }
        }

        public class UserHandler : IRequestHandler<SendChangePassCodeCommand, UserDT>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IOptionsSnapshot<AppsetingOption> _options;

            public UserHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo,
                IOptionsSnapshot<AppsetingOption> options)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _options = options;
            }
            public async Task<UserDT> Handle(SendChangePassCodeCommand request, CancellationToken cancellationToken)
            {
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                //User exist = db.User.Where(d => Utils.CheckPhone(phone, d.UserName) && d.Status != (int)Const.Status.DELETED).FirstOrDefault();
                User exist = await _userRepo.FindByPhoneUsernameAsync(request.phone, 1, cancellationToken);
                if (exist == null)
                {
                    //def.meta = new Meta(404, "Not Found");
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                exist.RegisterCode = Utils.RandomNumber(6);
                exist.LastLoginAt = DateTime.Now;
                exist.UpdatedAt = DateTime.Now;
                //db.User.Update(exist);
                //await db.SaveChangesAsync();
                _userRepo.Update(exist);
                var entity = _mapper.Map<User, UserDT>(exist);
                await _unitOfWork.CommitChangesAsync();
                string mes = "TNSPLUS: " + exist.RegisterCode + " la ma OTP doi mat khau cua ban";
                try
                {
                    ResSendOTP resSendOTP = new ResSendOTP();
                    resSendOTP.phone = request.phone;
                    resSendOTP.mess = mes;
                    resSendOTP.code = exist.RegisterCode;
                    resSendOTP.projectId = exist.ProjectId;
                    string baseApi = _options.Value.baseApi;
                    RestClient client = new RestClient(baseApi);
                    var requestA = new RestRequest("api/app/SMSBrandname/sendOTP2", Method.Post);
                    requestA.AddJsonBody(resSendOTP);
                    requestA.AddHeader("Content-Type", "application/json");
                    //requestA.AddHeader("token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c24iOiJ2bWd0ZXN0MSIsInNpZCI6ImZkMTA5NWM3LTc0NGQtNGUzZS1iODRlLTg2MWUzZDAzMmM2ZiIsIm9idCI6IiIsIm9iaiI6IiIsIm5iZiI6MTU4MzgzNDA3OCwiZXhwIjoxNTgzODM3Njc4LCJpYXQiOjE1ODM4MzQwNzh9.5wS0_iqzHypsZaGFTBtVyTCyHegSWj1onY-hQqw7b40");
                    var response = await client.ExecuteAsync(requestA);

                    var content = response.Content; // raw content as string
                    if (content != null)
                    {

                        JObject json = JObject.Parse(content);

                        if (json["meta"]["error_code"].ToString() == "200")
                        {
                            //user.UserName = json["data"]["sendMessage"]["to"].ToString();
                            //user.Phone = json["data"]["sendMessage"]["to"].ToString();
                            ////db.User.Update(user);
                            ////await db.SaveChangesAsync();
                            ////var entity = _mapper.Map<User>(user);
                            //_userRepo.Update(user);
                            //await _unitOfWork.CommitChangesAsync();
                            _unitOfWork.CommitTransaction();

                            return entity;
                        }
                        else
                        {
                            int code = int.Parse(json["meta"]["error_code"].ToString());
                            string message = json["meta"]["error_message"].ToString();
                            throw new CommonException($"Lấy mã OTP thất bại. Response: {json["meta"]["error_message"]}", 230, ApiConstants.ErrorCode.ERROR_OPT_GET_FAILED);
                        }
                    }
                    else
                    {
                        throw new CommonException($"Lấy mã OTP không thành công! Vui lòng thử lại.", 230, ApiConstants.ErrorCode.ERROR_OPT_GET_FAILED);
                    }
                }
                catch (Exception e)
                {
                    throw new CommonException($"Lấy mã OTP thất bại. ERROR: {e.Message}", 230, ApiConstants.ErrorCode.ERROR_OPT_GET_FAILED);
                };
                //ResponseCSKH sendSMS = await SendSMS.SendCSKH(exist.Phone, mes, exist.RegisterCode);
                //if (sendSMS != null)
                //{
                //    if (sendSMS.errorCode == "000")
                //    {
                //        transaction.Commit();
                //        def.meta = new Meta(200, "Success");
                //        def.data = exist;
                //        return Ok(def);
                //    }
                //    else
                //    {
                //        def.data = exist;
                //        def.meta = new Meta(230, "Not send sms");
                //        return Ok(def);
                //    }
                //}
                //else
                //{
                //    def.data = exist;
                //    def.meta = new Meta(230, "Not send sms");
                //    return Ok(def);
                //}

                //return entity;
                //}

                //return true;
            }
        }
    }
}
