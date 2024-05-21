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

namespace IOIT.Identity.Application.Towers.Commands.Update
{
    public class UpdateTowerCommand : IRequest<Tower>
    {
        public int Id { get; set; }
        public long? TowerId { get; set; }
        public int? OneSid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? Status { get; set; }


        public class Validation : AbstractValidator<UpdateTowerCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
                RuleFor(x => x.ProjectId).NotEmpty().WithMessage("ProjectId not empty").GreaterThan(0);
                RuleFor(x => x.TowerId).NotEmpty().WithMessage("TowerId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateTowerCommand, Tower>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Tower> _projectRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Tower> projectRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _projectRepository = projectRepository;
            }

            public async Task<Tower> Handle(UpdateTowerCommand request, CancellationToken cancellationToken)
            {
                var data = await _projectRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new BadRequestException("Dữ liệu tòa nhà không tồn tại trong hệ thống.", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_TOWER_NOT_EXIST);
                }

                var entity = _mapper.Map<Tower>(request);
                entity.UpdatedAt = DateTime.Now;
                entity.CreatedById = data.CreatedById;
                entity.UpdatedById = request.UpdatedById;

                _projectRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }
}
