using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Floors.Commands.Update
{
    public class UpdateFloorCommand : IRequest<Floor>
    {
        public int Id { get; set; }
        public int? OneSid { get; set; }
        public int? FloorId { get; set; }
        public int? TowerId { get; set; }
        public int? ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? Status { get; set; }

        public class Validation : AbstractValidator<UpdateFloorCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
                RuleFor(x => x.FloorId).NotEmpty().WithMessage("FloorId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateFloorCommand, Floor>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IFloorAsyncRepository _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IFloorAsyncRepository entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<Floor> Handle(UpdateFloorCommand request, CancellationToken cancellationToken)
            {
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new BadRequestException("Tầng thuộc tòa nhà không tồn tại trong hệ thống.", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_FLOOR_NOT_EXIST);
                }

                var entity = _mapper.Map<Floor>(request);
                entity.CreatedAt = data.CreatedAt;
                entity.CreatedById = data.CreatedById;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = request.UpdatedById;

                _entityRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }
}
