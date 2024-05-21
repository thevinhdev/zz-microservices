using AutoMapper;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Commands.Create
{
    public class CheckPhoneMainCommand : IRequest<ResUserRegisterApp>
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

        public class Validation : AbstractValidator<CheckPhoneMainCommand>
        {
            public Validation()
            {
                RuleFor(x => x.PhoneMain).NotEmpty().WithMessage("PhoneMain not empty");
                RuleFor(x => x.LanguageId).NotEmpty().WithMessage("LanguageId not empty");
            }
        }

        public class UserRegisterHandler : IRequestHandler<CheckPhoneMainCommand, ResUserRegisterApp>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IResidentAsyncRepository _residentRepo;

            public UserRegisterHandler(IMapper mapper, IUserAsyncRepository userRepo,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository residentRepo
                )
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _residentRepo = residentRepo;
            }

            public async Task<ResUserRegisterApp> Handle(CheckPhoneMainCommand request, CancellationToken cancellationToken)
            {
                //using (var db = new IOITResidentGateContext())
                //{
                //check xem sdt đó có phù hợp với số chủ hộ nào ko
                //var checkResident = await (from res in db.Resident
                //                           join ar in db.ApartmentMap on res.ResidentId equals ar.ResidentId
                //                           where ar.ProjectId == data.ProjectId && ar.TowerId == data.TowerId
                //                           && ar.FloorId == data.FloorId && ar.ApartmentId == data.ApartmentId
                //                           && ar.Type == (int)Const.TypeResident.RESIDENT_MAIN
                //                           && res.Status == (int)Const.Status.NORMAL
                //                           && ar.Status != (int)Const.Status.DELETED
                //                           select res).FirstOrDefaultAsync();
                var checkResident = await _residentRepo.CheckResidentExitAsync(request.ProjectId, request.TowerId,
               request.FloorId, request.ApartmentId, cancellationToken);

                if (checkResident == null)
                    {
                        //def.meta = new Meta(216, "Resident main not found!");
                        //return Ok(def);
                    throw new CommonException("Không tồn tại chủ hộ!", 216, ApiConstants.ErrorCode.ERROR_RESIDENTMAIN_NOT_EXIST);
                }

                    if (!Utils.CheckPhone(request.PhoneMain, checkResident.Phone))
                    {
                    //def.meta = new Meta(217, "Phone main not found!");
                    //return Ok(def);
                    throw new CommonException("Không tìm thấy số điện thoại chủ hộ!", 217, ApiConstants.ErrorCode.ERROR_RESIDENT_PHONE_NOT_FOUND);
                }

                //check xem tài khoản đã tồn tại chưa or đã active chưa or đã đổi mk mới chưa
                //var checkExistUn = await db.User.Where(e => Utils.CheckPhone(request.PhoneGuest, e.UserName) && e.Status != (int)Const.Status.DELETED).FirstOrDefaultAsync();
                var checkExistUn = await _userRepo.FindByPhoneUsernameAsync(request.PhoneMain, 1,cancellationToken);
                if (checkExistUn != null)
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
                    //def.data = checkExistUn;
                    //return Ok(def);
                    
                }
                //data.ResidentId = checkResident.ResidentId;
                //def.meta = new Meta(200, "Success");
                //def.data = data;
                //return Ok(def);
                return _mapper.Map<ResUserRegisterApp>(checkExistUn);
                //}

            }
        }
    }
}
