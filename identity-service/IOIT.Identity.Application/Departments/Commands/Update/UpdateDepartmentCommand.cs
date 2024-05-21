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

namespace IOIT.Identity.Application.Departments.Commands.Update
{
    public class UpdateDepartmentCommand : IRequest<Department>
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? Status { get; set; }

        public class Validation : AbstractValidator<UpdateDepartmentCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
                RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("DepartmentId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateDepartmentCommand, Department>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IDepartmentAsyncRepository _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IDepartmentAsyncRepository entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<Department> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
            {
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new BadRequestException("Phòng ban không tồn tại", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_DEPARTMENT_NOT_EXIST);
                }

                var entity = _mapper.Map<Department>(request);
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
