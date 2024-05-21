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

namespace IOIT.Identity.Application.Apartments.Commands.Create
{
    public class CreateApartmentCommand : IRequest<Apartment>
    {
        public int ApartmentId { get; set; }
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
        public int Status { get; set; }

        public class Validation : AbstractValidator<CreateApartmentCommand>
        {
            public Validation()
            {
                RuleFor(x => x.ApartmentId).NotEmpty().WithMessage("ApartmentId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<CreateApartmentCommand, Apartment>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApartmentAsyncRepository _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IApartmentAsyncRepository entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<Apartment> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
            {
                var data = _entityRepository.All().Where(e => e.ProjectId == request.ProjectId && e.TowerId == request.TowerId
                && e.FloorId == request.FloorId && e.ApartmentId == request.ApartmentId).FirstOrDefault();
                if (data == null)
                {
                    var entity = _mapper.Map<Apartment>(request);
                    await _entityRepository.AddAsync(entity);
                    await _unitOfWork.CommitChangesAsync();
                    return entity;
                }
                else
                {
                    var entity = _mapper.Map<Apartment>(data);
                    entity.ApartmentId = request.ApartmentId;
                    entity.FloorId = request.FloorId;
                    entity.TowerId = request.TowerId;
                    entity.ProjectId = request.ProjectId;
                    entity.Code = request.Code;
                    entity.Name = request.Name;
                    entity.UpdatedAt = DateTime.Now;
                    entity.UpdatedById = request.UpdatedById;
                    entity.Status = (EntityStatus)request.Status;
                    _entityRepository.Update(entity);
                    await _unitOfWork.CommitChangesAsync();
                    return entity;
                }
            }
        }
    }
}
