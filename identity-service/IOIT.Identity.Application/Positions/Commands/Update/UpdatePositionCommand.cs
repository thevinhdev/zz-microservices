using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Positions.Commands.Update
{
    public class UpdatePositionCommand : IRequest<Position>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ProjectId { get; set; }
        public int? TowerId { get; set; }
        public int? LevelId { get; set; }
        public string Note { get; set; }
        public long? UserId { get; set; }

        public class Validation : AbstractValidator<UpdatePositionCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Code).NotEmpty().WithMessage("Code not empty");
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdatePositionCommand, Position>
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

            public async Task<Position> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
            {
                if (request.Code == null)
                {
                    throw new CommonException("Mã không có dữ liệu!", 211, ApiConstants.ErrorCode.ERROR_CODE_EMPTY);
                }
                if (request.Code.Trim() == "")
                {
                    throw new CommonException("Mã không có dữ liệu!", 211, ApiConstants.ErrorCode.ERROR_CODE_EMPTY);
                }
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new NotFoundException("The Position does not exist.", Constants.StatusCodeResApi.Error404);
                }

                var checkCode = _entityRepository.All().AsNoTracking().Where(f => f.Code == request.Code && f.Id != request.Id && f.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                if (checkCode != null)
                {
                    throw new CommonException("Mã đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_CODE_EXISTED);
                }

                //var entity = _mapper.Map<Position>(request);
                //entity.CreatedAt = data.CreatedAt;
                data.Code = request.Code;
                data.Name = request.Name;
                data.LevelId = request.LevelId;
                data.Note = request.Note;
                data.ProjectId = request.ProjectId;
                data.TowerId = request.TowerId;
                data.UpdatedAt = DateTime.Now;
                //entity.CreatedById = data.CreatedById;
                data.UpdatedById = request.UserId;
                //entity.Status = data.Status;

                _entityRepository.Update(data);
                await _unitOfWork.CommitChangesAsync();

                return data;
            }
        }
    }
}
