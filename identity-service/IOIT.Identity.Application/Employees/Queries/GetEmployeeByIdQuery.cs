using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Employees.Queries
{
    public class GetEmployeeByIdQuery : IRequest<Employee>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetEmployeeByIdQuery, Employee>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Employee> _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Employee> entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _entityRepository.GetByKeyAsync(request.Id);

                //if (entity == null)
                //{
                //    throw new BadRequestException("The project does not exist.", Constants.StatusCodeResApi.Error404);
                //}

                return entity;
            }
        }
    }
}
