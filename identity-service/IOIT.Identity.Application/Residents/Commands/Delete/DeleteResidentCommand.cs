using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces.Producer;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Residents.Commands.Delete
{
    public class DeleteResidentCommand : IRequest<Resident>
    {
        public long Id { get; set; }
        public long? UserId { get; set; }

        public class Validation : AbstractValidator<DeleteResidentCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
            }

            
        }
        public class Handler : IRequestHandler<DeleteResidentCommand, Resident>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncLongRepository<Resident> _entityRepository;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IIdentityProducer _identityProducer;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncLongRepository<Resident> entityRepository,
                IApartmentMapAsyncRepository amRepo,
                IUserAsyncRepository userRepo,
                IIdentityProducer identityProducer)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _amRepo = amRepo;
                _userRepo = userRepo;
                _identityProducer = identityProducer;
            }

            public async Task<Resident> Handle(DeleteResidentCommand request, CancellationToken cancellationToken)
            {
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new NotFoundException("The Resident does not exist.", Constants.StatusCodeResApi.Error404);
                }

                if (data.Status == EntityStatus.DELETED)
                {
                    throw new NotFoundException("The Resident does not exist.", Constants.StatusCodeResApi.Error404);
                }

                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);

                data.UpdatedAt = DateTime.Now;
                data.UpdatedById = request.UserId;
                data.Status = AppEnum.EntityStatus.DELETED;

                _entityRepository.Update(data);
                await _unitOfWork.CommitChangesAsync();

                //Xóa map
                //var data1 = await db.ApartmentMap.AsNoTracking().Where(e => e.ResidentId == id && e.Status != (int)Const.Status.DELETED).ToListAsync();
                List<ApartmentMap> data1 = await _amRepo.GetListByResidentAsync(request.Id, cancellationToken);
                foreach (var item in data1)
                {
                    item.UpdatedAt = DateTime.Now;
                    item.UpdatedById = request.UserId;
                    item.Status = AppEnum.EntityStatus.DELETED;
                    //db.ApartmentMap.Update(item);

                    await _identityProducer.IdentityApartmentMapCreate(item);
                }
                _amRepo.UpdateRange(data1);

                //check xem cư dân có tài khoản ko, nếu có thì xóa luôn tài khoản user
                //var user = db.User.Where(e => e.UserMapId == data.ResidentId).FirstOrDefault();
                User user = await _userRepo.FindByResidentAsync(data.Id, cancellationToken);
                if (user != null)
                {
                    user.UpdatedAt = DateTime.Now;
                    user.Status = AppEnum.EntityStatus.DELETED;
                    //db.User.Update(user);
                    _userRepo.Update(user);
                }

                try
                {
                    await _unitOfWork.CommitChangesAsync();

                    if (data.Id > 0)
                    {
                        _unitOfWork.CommitTransaction();
                        //create action
                        //IOITResident.Models.EF.Action action = new IOITResident.Models.EF.Action();
                        //action.ActionId = Guid.NewGuid();
                        //action.ActionName = "Xóa cư dân " + data.FullName;
                        //action.ActionType = "DELETE";
                        //action.TargetId = data.ResidentId.ToString();
                        //action.TargetType = "Resident";
                        //action.Logs = JsonConvert.SerializeObject(data);
                        //action.Time = 0;
                        //action.Type = (int)Const.TypeAction.ACTION;
                        //action.CreatedAt = DateTime.Now;
                        //action.UserPushId = userId;
                        //action.UserId = userId;
                        //action.Status = (int)Const.Status.NORMAL;
                        //db.Action.Add(action);

                        //await db.SaveChangesAsync();
                        return data;
                    }
                    else
                    {
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_RESIDENT_DELETE_FAILED);
                    }
                }
                catch (DbUpdateException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_RESIDENT_DELETE_FAILED);
                }

                
            }
        }
    }
}
