using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Helpers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Commands.Update
{
    public class UserChangePassCommand : IRequest<User>
    {
        public long UserId { get; set; }
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }

        public class Validation : AbstractValidator<UserChangePassCommand>
        {
            public Validation()
            {
                RuleFor(x => x.PasswordOld).NotEmpty().WithMessage("PasswordOld not empty");
                RuleFor(x => x.PasswordNew).NotEmpty().WithMessage("PasswordNew not empty");
                RuleFor(x => x.PasswordNew).MinimumLength(6).WithMessage("Password phải có độ dài lớn hơn 6 ký tự");
            }
        }

        public class UserChangePassHandler : IRequestHandler<UserChangePassCommand, User>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;

            public UserChangePassHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
            }
            public async Task<User> Handle(UserChangePassCommand request, CancellationToken cancellationToken)
            {
                //User user = await db.User.FindAsync(id);
                User user = await _userRepo.GetByKeyAsync(request.UserId);
                if (user == null)
                {
                    //def.meta = new Meta(404, Const.NOT_FOUND_UPDATE_MESSAGE);
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                //check password old
                string password = user.KeyRandom.Trim() + user.RegEmail.Trim() + user.Id + request.PasswordOld.Trim();
                password = Utils.GetMD5Hash(password);
                if (user.Password.Trim() != password)
                {
                    //def.meta = new Meta(213, "Mật khẩu cũ không đúng!");
                    //return Ok(def);
                    throw new CommonException("Mật khẩu cũ không đúng!", 213, ApiConstants.ErrorCode.ERROR_PASSWORD_NEW_EXISTED);
                }

                //update user
                string passwordNew = user.KeyRandom.Trim() + user.RegEmail.Trim() + user.Id + request.PasswordNew.Trim();
                passwordNew = Utils.GetMD5Hash(passwordNew);

                user.Password = passwordNew;
                user.UpdatedAt = DateTime.Now;

                var entity = _mapper.Map<User>(user);

                _userRepo.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }
}
