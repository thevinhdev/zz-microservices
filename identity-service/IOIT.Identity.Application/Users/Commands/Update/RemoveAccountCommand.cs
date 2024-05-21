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
    public class RemoveAccountCommand : IRequest<RemoveAccountCommand>
    {
        public long UserId { get; set; }

        public class Validation : AbstractValidator<RemoveAccountCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty");
            }
        }

        public class UserHandler : IRequestHandler<RemoveAccountCommand, RemoveAccountCommand>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IResidentAsyncRepository _residentRepo;
            private readonly IEmployeeAsyncRepository _empRepo;

            public UserHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo,
                IResidentAsyncRepository residentRepo,
                IEmployeeAsyncRepository empRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _residentRepo = residentRepo;
                _empRepo = empRepo;
            }
            public async Task<RemoveAccountCommand> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
            {
                User exist = await _userRepo.GetByKeyAsync(request.UserId);
                if (exist == null)
                {
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                exist.IsDeletedByGuest = true;
                exist.UpdatedAt = DateTime.Now;
                var entity = _mapper.Map<User>(exist);
                _userRepo.Update(entity);
                await _unitOfWork.CommitChangesAsync();
                return request;

            }
        }
    }
}
