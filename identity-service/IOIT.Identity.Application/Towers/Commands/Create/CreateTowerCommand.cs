using AutoMapper;
using FluentValidation;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Towers.Commands.Create
{
    public class CreateTowerCommand : IRequest<Tower>
    {
        public int TowerId { get; set; }
        public int? OneSid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int Status { get; set; }

        public class Validation : AbstractValidator<CreateTowerCommand>
        {
            public Validation()
            {
                RuleFor(x => x.ProjectId).NotEmpty().WithMessage("ProjectId not empty").GreaterThan(0);
                RuleFor(x => x.TowerId).NotEmpty().WithMessage("TowerId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<CreateTowerCommand, Tower>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Tower> _towerRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Tower> towerRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _towerRepository = towerRepository;
            }

            public async Task<Tower> Handle(CreateTowerCommand request, CancellationToken cancellationToken)
            {
                var data = _towerRepository.All().Where(e => e.ProjectId == request.ProjectId && e.TowerId == request.TowerId).FirstOrDefault();
                if (data == null)
                {
                    var entity = _mapper.Map<Tower>(request);
                    await _towerRepository.AddAsync(entity);
                    await _unitOfWork.CommitChangesAsync();
                    return entity;
                }
                else
                {
                    var entity = _mapper.Map<Tower>(data);
                    entity.TowerId = request.TowerId;
                    entity.ProjectId = request.ProjectId;
                    entity.Code = request.Code;
                    entity.Name = request.Name;
                    entity.UpdatedAt = DateTime.Now;
                    entity.UpdatedById = request.UpdatedById;
                    entity.Status = (EntityStatus)request.Status;

                    _towerRepository.Update(entity);
                    await _unitOfWork.CommitChangesAsync();
                    return entity;
                }
            }
        }
    }

}