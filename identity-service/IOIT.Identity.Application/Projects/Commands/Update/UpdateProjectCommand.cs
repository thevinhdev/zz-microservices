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

namespace IOIT.Identity.Application.Projects.Commands.Update
{
    public class UpdateProjectCommand : IRequest<Project>
    {
        public int? ProjectId { get; set; }
        public int Id { get; set; }
        public int? OneSid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedById { get; set; }
        public int? Status { get; set; }

        public class Validation : AbstractValidator<UpdateProjectCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
                RuleFor(x => x.ProjectId).NotEmpty().WithMessage("ProjectId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateProjectCommand, Project>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Project> _projectRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Project> projectRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _projectRepository = projectRepository;
            }

            public async Task<Project> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
            {
                var data = await _projectRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new BadRequestException("Dự án không tồn tại trong hệ thống.", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_FUNCTION_NOT_EXIST);
                }

                var entity = _mapper.Map<Project>(request);
                entity.CreatedAt = data.CreatedAt;
                entity.UpdatedAt = DateTime.Now;
                entity.CreatedById = data.CreatedById;
                entity.UpdatedById = request.UpdatedById != null ? request.UpdatedById : data.UpdatedById;

                _projectRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }
}
