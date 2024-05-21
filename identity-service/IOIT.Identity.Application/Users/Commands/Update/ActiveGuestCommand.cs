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
    public class ActiveGuestCommand : IRequest<UserDT>
    {
        public long id { get; set; }

        public class Validation : AbstractValidator<ActiveGuestCommand>
        {
            public Validation()
            {
                RuleFor(x => x.id).NotEmpty().WithMessage("id not empty");
            }
        }

        public class UserHandler : IRequestHandler<ActiveGuestCommand, UserDT>
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
            public async Task<UserDT> Handle(ActiveGuestCommand request, CancellationToken cancellationToken)
            {
                //User exist = db.User.Where(d => d.UserId == id && d.Status != (int)Const.Status.DELETED).FirstOrDefault();
                User exist = await _userRepo.GetByKeyAsync(request.id);
                if (exist == null)
                {
                    //def.meta = new Meta(404, "Not Found");
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                exist.Status = AppEnum.EntityStatus.NORMAL;
                exist.UpdatedById = request.id;
                exist.UpdatedAt = DateTime.Now;
                //db.User.Update(exist);
                //await db.SaveChangesAsync();
                //transaction.Commit();

                //def.meta = new Meta(200, "Success");
                //def.data = exist;
                //return Ok(def);
                _userRepo.Update(exist);
                var entity = _mapper.Map<User, UserDT>(exist);
                await _unitOfWork.CommitChangesAsync();
                return entity;

            }
        }
    }
}
