using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Apartments.Commands.Update
{
    public class UpdateApartmentCommand : IRequest<Apartment>
    {
        public int Id { get; set; }
        public int? ApartmentId { get; set; }
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

        public class Validation : AbstractValidator<UpdateApartmentCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
                RuleFor(x => x.ApartmentId).NotEmpty().WithMessage("ApartmentId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }
        public class Handler : IRequestHandler<UpdateApartmentCommand, Apartment>
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

            public async Task<Apartment> Handle(UpdateApartmentCommand request, CancellationToken cancellationToken)
            {
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new BadRequestException("The project does not exist.", Constants.StatusCodeResApi.Error404);
                }

                var entity = _mapper.Map<Apartment>(request);
                entity.CreatedAt = data.CreatedAt;
                entity.UpdatedAt = DateTime.Now;
                entity.CreatedById = data.CreatedById;
                entity.UpdatedById = request.UpdatedById;

                _entityRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }
}
