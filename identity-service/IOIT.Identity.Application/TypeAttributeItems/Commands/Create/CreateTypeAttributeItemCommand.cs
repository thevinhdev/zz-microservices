using AutoMapper;
using FluentValidation;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.TypeAttributeItems.Commands.Create
{
    public class CreateTypeAttributeItemCommand : IRequest<TypeAttributeItem>
    {
        public int TypeAttributeItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? Status { get; set; }

        public class Validation : AbstractValidator<CreateTypeAttributeItemCommand>
        {
            public Validation()
            {
                RuleFor(x => x.TypeAttributeItemId).NotEmpty().WithMessage("TypeId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<CreateTypeAttributeItemCommand, TypeAttributeItem>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITypeAttributeItemAsyncRepository _towerRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                ITypeAttributeItemAsyncRepository towerRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _towerRepository = towerRepository;
            }

            public async Task<TypeAttributeItem> Handle(CreateTypeAttributeItemCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<TypeAttributeItem>(request);

                await _towerRepository.AddAsync(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }

}