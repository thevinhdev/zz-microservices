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
    public class GetEmployeeHandle : FilterPagination, IRequest<IPagedResult<ResEmpHandle>>
    {
        public int TowerId { get; set; }
        public int DepartmentId { get; set; }
        public int UserMapId { get; set; }
        public class Handler : IRequestHandler<GetEmployeeHandle, IPagedResult<ResEmpHandle>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmployeeAsyncRepository _employeeRepository;
            private readonly IEmployeeMapAsyncRepository _employeeMapRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IEmployeeAsyncRepository employeeRepository,
                IEmployeeMapAsyncRepository employeeMapRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _employeeRepository = employeeRepository;
                _employeeMapRepository = employeeMapRepository;
            }

            public async Task<IPagedResult<ResEmpHandle>> Handle(GetEmployeeHandle request, CancellationToken cancellationToken)
            {
                var data = (from e in _employeeRepository.All().Where(e => e.DepartmentId == request.DepartmentId && e.Id != request.UserMapId && e.Status != AppEnum.EntityStatus.DELETED)
                            join em in _employeeMapRepository.All().Where(em => em.TowerId == request.TowerId && em.Status != AppEnum.EntityStatus.DELETED) on e.Id equals em.EmployeeId
                            select new ResEmpHandle
                            {
                                EmployeeId = e.Id,
                                FullName = e.FullName
                            }).OrderBy(x => x.FullName).ToList();

                var result = new PagedResult<ResEmpHandle>
                {
                    RowCount = data.Count(),
                    Results = data
                };

                return result;

            }
        }
    }
}
