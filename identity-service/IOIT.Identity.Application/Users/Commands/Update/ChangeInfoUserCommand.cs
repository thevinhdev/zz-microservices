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
    public class ChangeInfoUserCommand : IRequest<User>
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Avata { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public class Validation : AbstractValidator<ChangeInfoUserCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty");
                RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName not empty");
            }
        }

        public class ChangeInfoUserHandler : IRequestHandler<ChangeInfoUserCommand, User>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;

            public ChangeInfoUserHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
            }
            public async Task<User> Handle(ChangeInfoUserCommand request, CancellationToken cancellationToken)
            {
                //User user = await db.User.FindAsync(id);
                User user = await _userRepo.GetByKeyAsync(request.UserId);
                if (user == null)
                {
                    //def.meta = new Meta(404, Const.NOT_FOUND_UPDATE_MESSAGE);
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                //update user
                user.FullName = request.FullName;
                user.Email = request.Email;
                user.Code = request.Code;
                user.Avata = request.Avata != null && request.Avata != "" ? request.Avata : user.Avata;
                user.Address = request.Address;
                user.Phone = request.Phone;
                user.UpdatedAt = DateTime.Now;
                //user.UpdatedById = userId;
                var entity = _mapper.Map<User>(user);

                _userRepo.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }
}
