using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Residents.Commands.Update
{
    public class UpdateStatusResidentCommand : IRequest<Resident>
    {
        public long Id { get; set; }
        public EntityStatus Status { get; set; }
        public long UserId { get; set; }

        public class Handler : IRequestHandler<UpdateStatusResidentCommand, Resident>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly IApartmentAsyncRepository _apartRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository,
                IApartmentMapAsyncRepository amRepo,
                IApartmentAsyncRepository apartRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _amRepo = amRepo;
                _apartRepo = apartRepo;
            }

            public async Task<Resident> Handle(UpdateStatusResidentCommand request, CancellationToken cancellationToken)
            {
                var resident = await _entityRepository.GetByKeyAsync(request.Id);
                if (resident == null)
                {
                    throw new BadRequestException("Dữ liệu cư dân không tồn tại trong hệ thống.", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_RESIDENT_NOT_EXIST);
                }

                resident.Status = request.Status;
                resident.UpdatedById = request.UserId;
                resident.UpdatedAt = DateTime.Now;

                _entityRepository.Update(resident);
                await _unitOfWork.CommitChangesAsync();

                if (request.Status == EntityStatus.DELETED)
                {
                    List<ApartmentMap> data1 = await _amRepo.GetListByResidentAsync(request.Id, cancellationToken);
                    foreach (var item in data1)
                    {
                        item.UpdatedAt = DateTime.Now;
                        item.UpdatedById = request.UserId;
                        item.Status = EntityStatus.DELETED;
                    }

                    _amRepo.UpdateRange(data1);
                    await _unitOfWork.CommitChangesAsync();
                }

                return resident;
            }
        }
    }
}
