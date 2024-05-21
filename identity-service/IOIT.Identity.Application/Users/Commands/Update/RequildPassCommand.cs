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
    public class RequildPassCommand : IRequest<User>
    {
        public string Email { get; set; }
        public string Phone { get; set; }

        public class Validation : AbstractValidator<RequildPassCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone not empty");
            }
        }

        public class UserHandler : IRequestHandler<RequildPassCommand, User>
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
            public async Task<User> Handle(RequildPassCommand request, CancellationToken cancellationToken)
            {
                //User exist = db.User.Where(d => d.UserName.Trim().Equals(data.Phone.Trim()) && d.Status != (int)Const.Status.DELETED).FirstOrDefault();
                User exist = await _userRepo.FindByPhoneUsernameAsync(request.Phone, 1, cancellationToken);
                if (exist == null)
                {
                    //def.meta = new Meta(404, "Not Found");
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                //exist.Status = AppEnum.EntityStatus.NORMAL;
                //exist.UpdatedById = request.id;
                exist.UpdatedAt = DateTime.Now;
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
