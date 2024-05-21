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
    public class LockUserCommand : IRequest<Boolean>
    {
        public long id { get; set; }
        public int k { get; set; }

        public class Validation : AbstractValidator<LockUserCommand>
        {
            public Validation()
            {
                RuleFor(x => x.id).NotEmpty().WithMessage("id not empty");
                RuleFor(x => x.k).NotEmpty().WithMessage("k not empty");
            }
        }

        public class LockUserHandler : IRequestHandler<LockUserCommand, Boolean>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;

            public LockUserHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
            }
            public async Task<bool> Handle(LockUserCommand request, CancellationToken cancellationToken)
            {
                //User user = await db.User.FindAsync(id);
                User user = await _userRepo.GetByKeyAsync(request.id);
                if (user == null)
                {
                    //def.meta = new Meta(404, Const.NOT_FOUND_UPDATE_MESSAGE);
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                user.UpdatedAt = DateTime.Now;
                //user.UpdatedById = userId;
                user.Status = (AppEnum.EntityStatus)request.k;
                if (user.Status == AppEnum.EntityStatus.NORMAL)
                {
                    user.IsPhoneConfirm = true;
                    user.IsEmailConfirm = true;
                }
                else
                {
                    user.IsPhoneConfirm = false;
                    user.IsEmailConfirm = false;
                }

                var entity = _mapper.Map<User>(user);

                _userRepo.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return true;
            }
        }
    }
}
