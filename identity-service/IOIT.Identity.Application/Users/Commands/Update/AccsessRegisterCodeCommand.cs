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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Commands.Update
{
    public class AccsessRegisterCodeCommand : IRequest<UserDT>
    {
        public long id { get; set; }
        public string code { get; set; }

        public class Validation : AbstractValidator<AccsessRegisterCodeCommand>
        {
            public Validation()
            {
                RuleFor(x => x.id).NotEmpty().WithMessage("id not empty");
                RuleFor(x => x.code).NotEmpty().WithMessage("k not empty");
            }
        }

        public class UserHandler : IRequestHandler<AccsessRegisterCodeCommand, UserDT>
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
            public async Task<UserDT> Handle(AccsessRegisterCodeCommand request, CancellationToken cancellationToken)
            {
                User exist = _userRepo.All().Where(d => d.Id == request.id && d.RegisterCode == request.code && d.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                //User exist = await _userRepo.FindByRegisterCode(request.id, request.code, cancellationToken);
                if (exist == null)
                {
                    //def.meta = new Meta(404, "Not Found");
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }
                else
                {
                    TimeSpan timeCheck = DateTime.Now - exist.LastLoginAt.Value;
                    //Check xem đã đến thới gian gửi mail chưa
                    double countDays = timeCheck.TotalMinutes;
                    if (countDays > 3)
                    {
                        //def.meta = new Meta(229, "Over time");
                        //return Ok(def);
                        throw new CommonException("OVER TIME!", 229, ApiConstants.ErrorCode.ERROR_TIMEOUT);
                    }
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                    exist.IsPhoneConfirm = true;
                    exist.UpdatedById = request.id;
                    exist.UpdatedAt = DateTime.Now;
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
                //}

                //return true;
            }
        }

    }
}
