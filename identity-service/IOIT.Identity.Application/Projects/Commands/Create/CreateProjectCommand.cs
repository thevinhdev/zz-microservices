using AutoMapper;
using FluentValidation;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Projects.Commands.Create
{
    public class CreateProjectCommand : IRequest<Project>
    {
        public int ProjectId { get; set; }
        public int? OneSid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int Status { get; set; }

        public class Validation : AbstractValidator<CreateProjectCommand>
        {
            public Validation()
            {
                RuleFor(x => x.ProjectId).NotEmpty().WithMessage("ProjectId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
                //RuleFor(x => x.CreatedById).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                //RuleFor(x => x.UpdatedById).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<CreateProjectCommand, Project>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IProjectAsyncRepository _dataRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IProjectAsyncRepository dataRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _dataRepository = dataRepository;
            }

            public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
            {
                string condition = "ProjectId=" + request.ProjectId;
                var data = await _dataRepository.GetDataByCondition(condition);

                if (data == null)
                {
                    var entity = _mapper.Map<Project>(request);

                    await _dataRepository.AddAsync(entity);
                    await _unitOfWork.CommitChangesAsync();

                    return entity;
                }
                else
                {
                    var entity = _mapper.Map<Project>(data);
                    entity.ProjectId = request.ProjectId;
                    entity.OneSId = request.OneSid;
                    entity.Code = request.Code;
                    entity.Name = request.Name;
                    entity.UpdatedAt = DateTime.Now;
                    entity.UpdatedById = request.UpdatedById != null ? request.UpdatedById : data.UpdatedById;
                    entity.Status = (EntityStatus)request.Status;
                    _dataRepository.Update(entity);
                    await _unitOfWork.CommitChangesAsync();
                }

                return null;
            }
        }
    }
}
