using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
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
    public class ChangePasswordCommand : IRequest<User>
    {
        public long id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public class Validation : AbstractValidator<ChangePasswordCommand>
        {
            public Validation()
            {
                RuleFor(x => x.id).NotEmpty().WithMessage("id not empty");
            }
        }

        public class UserHandler : IRequestHandler<ChangePasswordCommand, User>
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
            public async Task<User> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                //User exist = db.User.Where(d => d.UserId == id && d.Status != (int)Const.Status.DELETED).FirstOrDefault();
                User exist = await _userRepo.GetByKeyAsync(request.id);
                if (exist == null)
                {
                    //def.meta = new Meta(404, "Not Found");
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }
                string password = exist.KeyRandom.Trim() + exist.RegEmail.Trim() + exist.Id + Utils.GetMD5Hash(request.CurrentPassword.Trim());
                password = Utils.GetMD5Hash(password);
                if (exist.Password.Trim() != password)
                {
                    //def.meta = new Meta(213, "Not Exist Password Old");
                    //return Ok(def);
                    throw new CommonException("Mật khẩu cũ không đúng!", 213, ApiConstants.ErrorCode.ERROR_PASSWORD_NEW_EXISTED);
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
                var entity = _mapper.Map<User>(exist);

                _userRepo.Update(entity);
                await _unitOfWork.CommitChangesAsync();
                return entity;

            }
        }
    }
}
