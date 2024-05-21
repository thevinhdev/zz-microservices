using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces.Producer;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Residents.Commands.Delete
{
    public class DeletesResidentCommand : IRequest<List<Resident>>
    {
        public long[] data { get; set; }
        public long UserId { get; set; }

        public class Validation : AbstractValidator<DeletesResidentCommand>
        {
            public Validation()
            {
                RuleFor(x => x.data).NotEmpty().WithMessage("data not empty");
            }
        }
        public class Handler : IRequestHandler<DeletesResidentCommand, List<Resident>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;
            private readonly IEmployeeAsyncRepository _empRepo;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly IIdentityProducer _identityProducer;
            private readonly IUserAsyncRepository _userRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository,
                IEmployeeAsyncRepository empRepo,
                IApartmentMapAsyncRepository amRep,
                IIdentityProducer identityProducer,
                IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _empRepo = empRepo;
                _amRepo = amRep;
                _identityProducer = identityProducer;
                _userRepo = userRepo;
            }

            public async Task<List<Resident>> Handle(DeletesResidentCommand request, CancellationToken cancellationToken)
            {
                List<Resident> listData = new List<Resident>();
                //bool hasDelete = false;
                for (int i = 0; i < request.data.Count(); i++)
                {
                    var resident = await _entityRepository.GetByKeyAsync(request.data[i]);

                    if (resident == null)
                    {
                        continue;
                    }

                    resident.UpdatedById = request.UserId;
                    resident.UpdatedAt = DateTime.Now;
                    resident.Status = AppEnum.EntityStatus.DELETED;
                    _entityRepository.Update(resident);

                    List<ApartmentMap> data1 = await _amRepo.GetListByResidentAsync(request.data[i], cancellationToken);
                    foreach (var item in data1)
                    {
                        item.UpdatedAt = DateTime.Now;
                        item.UpdatedById = request.UserId;
                        item.Status = AppEnum.EntityStatus.DELETED;
                        //db.ApartmentMap.Update(item);

                        await _identityProducer.IdentityApartmentMapCreate(item);
                    }
                    _amRepo.UpdateRange(data1);

                    User user = await _userRepo.FindByResidentAsync(request.data[i], cancellationToken);
                    if (user != null)
                    {
                        user.UpdatedAt = DateTime.Now;
                        user.Status = AppEnum.EntityStatus.DELETED;
                        //db.User.Update(user);
                        _userRepo.Update(user);
                    }

                    //create action
                    //IOITResident.Models.EF.Action action = new IOITResident.Models.EF.Action();
                    //action.ActionId = Guid.NewGuid();
                    //action.ActionName = "Xóa cư dân " + resident.FullName;
                    //action.ActionType = "DELETE";
                    //action.TargetId = resident.ResidentId.ToString();
                    //action.TargetType = "Resident";
                    //action.Logs = JsonConvert.SerializeObject(resident);
                    //action.Time = 0;
                    //action.Type = (int)Const.TypeAction.ACTION;
                    //action.CreatedAt = DateTime.Now;
                    //action.UserPushId = userId;
                    //action.UserId = userId;
                    //action.Status = (int)Const.Status.NORMAL;
                    //db.Action.Add(action);
                    await _unitOfWork.CommitChangesAsync();
                    listData.Add(resident);
                }

                return listData;
            }
        }
    }
}
