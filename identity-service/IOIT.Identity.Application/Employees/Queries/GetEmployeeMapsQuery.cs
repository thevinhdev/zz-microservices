using AutoMapper;
using IOIT.Identity.Application.Employees.ViewModels;
using IOIT.Identity.Application.Models;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Employees.Queries
{
    public class GetEmployeeMapsQuery: IRequest<List<Shared.ViewModels.EmployeeMap>>
    {
        public class Handler : IRequestHandler<GetEmployeeMapsQuery, List<Shared.ViewModels.EmployeeMap>>
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

            public async Task<List<Shared.ViewModels.EmployeeMap>> Handle(GetEmployeeMapsQuery request, CancellationToken cancellationToken)
            {
                var result = (from em in _employeeMapRepository.All().Where(em => em.Status != AppEnum.EntityStatus.DELETED)
                            select new Shared.ViewModels.EmployeeMap
                            {
                                EmployeeId = em.EmployeeId,
                                TowerId = em.TowerId
                            }).ToList();

                return result;

            }
        }
    }
}
