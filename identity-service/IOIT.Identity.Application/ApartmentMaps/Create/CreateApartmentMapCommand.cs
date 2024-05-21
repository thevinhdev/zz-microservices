using AutoMapper;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.ApartmentMaps.Create
{
    public class CreateApartmentMapCommand : IRequest<ApartmentMap>
    {
        public Guid Id { get; set; }
        public int? ApartmentId { get; set; }
        public int? FloorId { get; set; }
        public int? TowerId { get; set; }
        public int? ProjectId { get; set; }
        public long? ResidentId { get; set; }
        public byte? Type { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
        public EntityStatus? Status { get; set; }

        public class Handler : IRequestHandler<CreateApartmentMapCommand, ApartmentMap>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApartmentMapAsyncRepository _apartmentMapRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IApartmentMapAsyncRepository apartmentMapRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _apartmentMapRepository = apartmentMapRepository;
            }

            public async Task<ApartmentMap> Handle(CreateApartmentMapCommand request, CancellationToken cancellationToken)
            {
                var dtApartmentMap = await _apartmentMapRepository.GetByKeyAsync(request.Id);

                if (dtApartmentMap != null)
                {
                    dtApartmentMap.ProjectId = request.ProjectId;
                    dtApartmentMap.TowerId = request.ProjectId;
                    dtApartmentMap.FloorId = request.ProjectId;
                    dtApartmentMap.ApartmentId = request.ProjectId;
                    dtApartmentMap.Type = request.Type;
                    dtApartmentMap.Status = (EntityStatus)request.Status;
                    dtApartmentMap.ResidentId = request.ResidentId;
                    dtApartmentMap.UpdatedAt = DateTime.Now;
                    dtApartmentMap.UpdatedById = request.UserId;

                    _apartmentMapRepository.Update(dtApartmentMap);
                    await _unitOfWork.CommitChangesAsync();

                    return dtApartmentMap;
                }

                //var entity = _mapper.Map<ApartmentMap>(request);

                //await _apartmentMapRepository.AddAsync(entity);

                //await _unitOfWork.CommitChangesAsync();

                return new ApartmentMap();
            }
        }
    }
}
