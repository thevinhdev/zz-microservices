using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Residents.Queries
{
    public class GetListResidentByIdQuery : IRequest<List<Resident>>
    {
        public long Id { get; set; }

        public class Handler : IRequestHandler<GetListResidentByIdQuery, List<Resident>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;
            private readonly IApartmentMapAsyncRepository _amRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository,
                IApartmentMapAsyncRepository amRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _amRepository = amRepository;
            }

            public async Task<List<Resident>> Handle(GetListResidentByIdQuery request, CancellationToken cancellationToken)
            {
            //    var data = (from r in _entityRepository.All()
            //     join ap in _amRepository.All() on r.ResidentId equals ap.ResidentId
            //     where r.Status != (int)Const.Status.DELETED
            //     && ap.Status != (int)Const.Status.DELETED
            //     && ap.ProjectId == notificationItem.TargetId
            //     group r by r.ResidentId into x
            //     select new Resident
            //     {
            //         ResidentId = x.FirstOrDefault().ResidentId,
            //     }).ToList();
                //var entity = await _entityRepository.GetByKeyAsync(request.Id);

                //if (entity == null)
                //{
                //    throw new BadRequestException("The Resident does not exist.", Constants.StatusCodeResApi.Error404);
                //}

                return null;
            }
        }
    }
}
