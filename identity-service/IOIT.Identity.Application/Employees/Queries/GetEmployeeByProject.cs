using AutoMapper;
using IOIT.Identity.Application.Employees.ViewModels;
using IOIT.Identity.Application.Models;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Employees.Queries
{
    public class GetEmployeeByProject : FilterPagination, IRequest<IPagedResult<ResEmpProject>>
    {
        public int ProjectId { get; set; }
        public class Handler : IRequestHandler<GetEmployeeByProject, IPagedResult<ResEmpProject>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmployeeAsyncRepository _employeeRepository;
            private readonly IAsyncRepository<Position> _positionRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IEmployeeAsyncRepository employeeRepository,
                IAsyncRepository<Position> positionRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _employeeRepository = employeeRepository;
                _positionRepository = positionRepository;
            }

            public async Task<IPagedResult<ResEmpProject>> Handle(GetEmployeeByProject request, CancellationToken cancellationToken)
            {
                var data = (from e in _employeeRepository.All().Where(e => e.Status != AppEnum.EntityStatus.DELETED)
                            join em in _positionRepository.All().Where(em => em.Status != AppEnum.EntityStatus.DELETED) on e.PositionId equals em.Id
                            select new ResEmpProject
                            {
                                EmployeeId = e.Id,
                                EmployeeCode = e.Code,
                                EmployeeName = e.FullName,
                                PositionName = em.Name,
                            }).OrderBy(x => x.EmployeeId).ToList();

                var result = new PagedResult<ResEmpProject>
                {
                    RowCount = data.Count(),
                    Results = data
                };

                return result;

            }
        }
    }
}
