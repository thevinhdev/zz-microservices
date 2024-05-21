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

namespace IOIT.Identity.Application.Departments.Commands.Create
{
    public class CreateDepartmentCommand : IRequest<Department>
    {
        public int DepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? Status { get; set; }

        public class Validation : AbstractValidator<CreateDepartmentCommand>
        {
            public Validation()
            {
                RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("DepartmentId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<CreateDepartmentCommand, Department>
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

            public async Task<Department> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
            {
                var data = _entityRepository.All().Where(e => e.DepartmentId == request.DepartmentId && e.ProjectId == request.ProjectId).FirstOrDefault();
                if (data == null)
                {
                    var entity = _mapper.Map<Department>(request);
                    await _entityRepository.AddAsync(entity);
                    await _unitOfWork.CommitChangesAsync();
                    return entity;
                }
                else
                {
                    var entity = _mapper.Map<Department>(data);
                    entity.DepartmentId = request.DepartmentId;
                    entity.Code = request.Code;
                    entity.Name = request.Name;
                    entity.ProjectId = request.ProjectId;
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
