using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Positions.Commands.Create
{
    public class CreatePositionCommand : IRequest<Position>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ProjectId { get; set; }
        public int? TowerId { get; set; }
        public int? LevelId { get; set; }
        public string Note { get; set; }
        public long? UserId { get; set; }

        public class Validation : AbstractValidator<CreatePositionCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Code).NotEmpty().WithMessage("Code not empty");
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<CreatePositionCommand, Position>
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

            public async Task<Position> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
            {
                if (request.Code == null)
                {
                    throw new CommonException("Mã không có dữ liệu!", 211, ApiConstants.ErrorCode.ERROR_CODE_EMPTY);
                }
                if (request.Code.Trim() == "")
                {
                    throw new CommonException("Mã không có dữ liệu!", 211, ApiConstants.ErrorCode.ERROR_CODE_EMPTY);
                }
                var checkCode = _entityRepository.All().AsNoTracking().Where(f => f.Code == request.Code && f.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                if (checkCode != null)
                {
                    throw new CommonException("Mã đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_CODE_EXISTED);
                }
                var entity = _mapper.Map<Position>(request);
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;
                entity.CreatedById = request.UserId;
                entity.UpdatedById = request.UserId;
                entity.Status = AppEnum.EntityStatus.NORMAL;
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                await _entityRepository.AddAsync(entity);
                await _unitOfWork.CommitChangesAsync();
                _unitOfWork.CommitTransaction();
                return entity;
            }
        }
    }
}
