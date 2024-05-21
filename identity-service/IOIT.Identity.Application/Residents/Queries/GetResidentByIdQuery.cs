using AutoMapper;
using FluentValidation;
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

namespace IOIT.Identity.Application.Residents.Queries
{
    public class GetResidentByIdQuery : IRequest<Resident>
    {
        public long Id { get; set; }

        public class Handler : IRequestHandler<GetResidentByIdQuery, Resident>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncLongRepository<Resident> _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncLongRepository<Resident> entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<Resident> Handle(GetResidentByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _entityRepository.GetByKeyAsync(request.Id);

                if (entity == null)
                {
                    throw new BadRequestException("Dữ liệu cư dân không tồn tại trong hệ thống.", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_RESIDENT_NOT_EXIST);
                }

                return entity;
            }
        }
    }
}
