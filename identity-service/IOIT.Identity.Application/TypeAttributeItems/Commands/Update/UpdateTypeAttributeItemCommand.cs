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

namespace IOIT.Identity.Application.TypeAttributeItems.Commands.Update
{
    public class UpdateTypeAttributeItemCommand : IRequest<TypeAttributeItem>
    {
        public int Id { get; set; }
        public int TypeAttributeItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? Status { get; set; }

        public class Validation : AbstractValidator<UpdateTypeAttributeItemCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
                RuleFor(x => x.TypeAttributeItemId).NotEmpty().WithMessage("TypeId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateTypeAttributeItemCommand, TypeAttributeItem>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITypeAttributeItemAsyncRepository _projectRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                ITypeAttributeItemAsyncRepository projectRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _projectRepository = projectRepository;
            }

            public async Task<TypeAttributeItem> Handle(UpdateTypeAttributeItemCommand request, CancellationToken cancellationToken)
            {
                var data = await _projectRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new BadRequestException("The type does not exist.", Constants.StatusCodeResApi.Error404);
                }

                var entity = _mapper.Map<TypeAttributeItem>(request);
                entity.CreatedAt = data.CreatedAt;
                entity.CreatedById = data.CreatedById;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = request.UpdatedById;

                _projectRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }
}
