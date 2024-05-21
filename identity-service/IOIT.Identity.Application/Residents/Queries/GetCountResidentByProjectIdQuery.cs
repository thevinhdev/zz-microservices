using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Residents.Queries
{
    public class GetCountResidentByProjectIdQuery : IRequest<int>
    {
        public int ProjectId { get; set; }

        public class Handler : IRequestHandler<GetCountResidentByProjectIdQuery, int>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _residentRepository;

            public Handler(IUnitOfWork unitOfWork,
                IResidentAsyncRepository residentRepository)
            {
                _unitOfWork = unitOfWork;
                _residentRepository = residentRepository;
            }

            public async Task<int> Handle(GetCountResidentByProjectIdQuery request, CancellationToken cancellationToken)
            {
                return await _residentRepository.GetCountResidentByProjectId(request.ProjectId, cancellationToken);
            }
        }
    }
}
