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
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Positions.Commands.Delete
{
    public class DeletePositionCommand : IRequest<Position>
    {
        public int Id { get; set; }
        public long? UserId { get; set; }

        public class Validation : AbstractValidator<DeletePositionCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
            }

            
        }
        public class Handler : IRequestHandler<DeletePositionCommand, Position>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Position> _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Position> entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<Position> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
            {
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new NotFoundException("The Position does not exist.", Constants.StatusCodeResApi.Error404);
                }

                if (data.Status == EntityStatus.DELETED)
                {
                    throw new NotFoundException("The Position does not exist.", Constants.StatusCodeResApi.Error404);
                }

                data.UpdatedAt = DateTime.Now;
                data.UpdatedById = request.UserId;
                data.Status = EntityStatus.DELETED;

                var entity = _mapper.Map<Position>(data);
                _entityRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return data;
            }
        }
    }
}
