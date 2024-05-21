using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Models;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Commands.Update
{
    public class NewPasswordCommand : IRequest<UserDT>
    {
        public long id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string RegisterCode { get; set; }

        public class Validation : AbstractValidator<NewPasswordCommand>
        {
            public Validation()
            {
                RuleFor(x => x.id).NotEmpty().WithMessage("id not empty");
            }
        }

        public class UserHandler : IRequestHandler<NewPasswordCommand, UserDT>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;

            public UserHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
            }
            public async Task<UserDT> Handle(NewPasswordCommand request, CancellationToken cancellationToken)
            {
                //User exist = db.User.Where(d => d.UserId == id && d.Status != (int)Const.Status.DELETED).FirstOrDefault();
                User exist = await _userRepo.GetByKeyAsync(request.id);
                if (exist == null)
                {
                    //def.meta = new Meta(404, "Not Found");
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }
                if (exist.RegisterCode == null || exist.RegisterCode == "")
                {
                    throw new CommonException("Tài khoản chưa được kích hoạt!", 218, ApiConstants.ErrorCode.ERROR_USER_NOT_ACTIVE);
                }
                if (exist.RegisterCode != null && exist.IsPhoneConfirm != true)
                {
                    //def.meta = new Meta(218, "Account Not Active!");
                    //return Ok(def);
                    throw new CommonException("Tài khoản chưa được kích hoạt!", 218, ApiConstants.ErrorCode.ERROR_USER_NOT_ACTIVE);
                }
                if (request.RegisterCode != null && request.RegisterCode != "")
                {
                    if (exist.RegisterCode != request.RegisterCode)
                    {
                        throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                    }
                }


                //using (var transaction = db.Database.BeginTransaction())
                //{
                string passNew = exist.KeyRandom.Trim() + exist.RegEmail.Trim() + exist.Id + Utils.GetMD5Hash(request.NewPassword);
                exist.Password = Utils.GetMD5Hash(passNew);
                exist.UpdatedAt = DateTime.Now;
                exist.UpdatedById = request.id;
                //db.User.Update(exist);
                //await db.SaveChangesAsync();
                //transaction.Commit();

                //def.meta = new Meta(200, "Success");
                //def.data = exist;
                //return Ok(def);
                

                _userRepo.Update(exist);
                await _unitOfWork.CommitChangesAsync();
                var entity = _mapper.Map<User, UserDT>(exist);
                return entity;

            }
        }
    }
}
