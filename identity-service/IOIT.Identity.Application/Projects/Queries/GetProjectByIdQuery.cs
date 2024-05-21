using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Projects.Queries
{
    public class GetProjectByIdQuery : IRequest<Project>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetProjectByIdQuery, Project>
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

            public async Task<Project> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _projectRepository.GetByKeyAsync(request.Id);

                if (entity == null)
                {
                    throw new BadRequestException("Dự án không tồn tại trong hệ thống", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_PROJECT_NOT_EXIST);
                }

                return entity;
            }
        }
    }
}
